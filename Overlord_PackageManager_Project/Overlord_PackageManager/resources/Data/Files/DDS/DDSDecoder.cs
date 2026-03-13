namespace Overlord_PackageManager.resources.Data.Files.DDS
{
    public static class DDSDecoder
    {
        public static byte[] Decode(uint width, uint height, DDSFormat format, byte[] data)
        {
            return format switch
            {
                DDSFormat.UncompressedRGB => DecodeRGB(width, height, data),
                DDSFormat.UncompressedRGBA => DecodeRGBA(width, height, data),
                DDSFormat.DXT1 => DecodeDXT(width, height, data, false, false),
                DDSFormat.DXT3 => DecodeDXT(width, height, data, true, false),
                DDSFormat.DXT5 => DecodeDXT(width, height, data, false, true),
                _ => throw new NotSupportedException()
            };
        }

        private static byte[] DecodeRGB(uint width, uint height, byte[] data)
        {
            byte[] result = new byte[width * height * 4];

            for (int i = 0, j = 0; i < data.Length; i += 3, j += 4)
            {
                // Convert RGB → BGRA
                result[j + 0] = data[i + 2]; // B
                result[j + 1] = data[i + 1]; // G
                result[j + 2] = data[i + 0]; // R
                result[j + 3] = 255;         // A
            }

            return result;
        }

        private static byte[] DecodeRGBA(uint width, uint height, byte[] data)
        {
            byte[] result = new byte[width * height * 4];

            for (int i = 0; i < data.Length; i += 4)
            {
                // Convert RGBA → BGRA
                result[i + 0] = data[i + 2]; // B
                result[i + 1] = data[i + 1]; // G
                result[i + 2] = data[i + 0]; // R
                result[i + 3] = data[i + 3]; // A
            }

            return result;
        }

        private static byte[] DecodeDXT(uint width, uint height, byte[] data, bool isDXT3, bool isDXT5)
        {
            int blockSize = isDXT3 || isDXT5 ? 16 : 8;

            int blocksWide = (int)((width + 3) / 4);
            int blocksHigh = (int)((height + 3) / 4);

            byte[] result = new byte[width * height * 4];

            int dataIndex = 0;

            for (int by = 0; by < blocksHigh; by++)
            {
                for (int bx = 0; bx < blocksWide; bx++)
                {
                    DecodeDXTBlock(
                        data,
                        dataIndex,
                        result,
                        (int)width,
                        (int)height,
                        bx * 4,
                        by * 4,
                        isDXT3,
                        isDXT5);

                    dataIndex += blockSize;
                }
            }

            return result;
        }

        private static void DecodeDXTBlock(
            byte[] data,
            int index,
            byte[] output,
            int width,
            int height,
            int startX,
            int startY,
            bool isDXT3,
            bool isDXT5)
        {
            byte[] alphaTable = null;
            ulong alphaBits = 0;

            if (isDXT3)
            {
                alphaBits = BitConverter.ToUInt64(data, index);
                index += 8;
            }
            else if (isDXT5)
            {
                byte a0 = data[index];
                byte a1 = data[index + 1];

                alphaBits = 0;
                for (int i = 0; i < 6; i++)
                    alphaBits |= (ulong)data[index + 2 + i] << 8 * i;

                index += 8;

                alphaTable = new byte[8];
                alphaTable[0] = a0;
                alphaTable[1] = a1;

                if (a0 > a1)
                {
                    for (int i = 2; i < 8; i++)
                        alphaTable[i] = (byte)(((8 - i) * a0 + (i - 1) * a1) / 7);
                }
                else
                {
                    for (int i = 2; i < 6; i++)
                        alphaTable[i] = (byte)(((6 - i) * a0 + (i - 1) * a1) / 5);

                    alphaTable[6] = 0;
                    alphaTable[7] = 255;
                }
            }

            ushort c0 = BitConverter.ToUInt16(data, index);
            ushort c1 = BitConverter.ToUInt16(data, index + 2);
            uint colorBits = BitConverter.ToUInt32(data, index + 4);

            Span<byte> colors = stackalloc byte[16 * 4];
            DecodeColorBlock(c0, c1, colorBits, colors);

            for (int py = 0; py < 4; py++)
            {
                for (int px = 0; px < 4; px++)
                {
                    int pixelIndex = py * 4 + px;

                    int outX = startX + px;
                    int outY = startY + py;

                    if (outX >= width || outY >= height)
                        continue;

                    int outIndex = (outY * width + outX) * 4;

                    output[outIndex + 0] = colors[pixelIndex * 4 + 0];
                    output[outIndex + 1] = colors[pixelIndex * 4 + 1];
                    output[outIndex + 2] = colors[pixelIndex * 4 + 2];

                    byte alpha = 255;

                    if (isDXT3)
                    {
                        alpha = (byte)((alphaBits >> pixelIndex * 4 & 0xF) * 17);
                    }
                    else if (isDXT5)
                    {
                        int alphaIndex = (int)(alphaBits >> pixelIndex * 3 & 0x7);
                        alpha = alphaTable[alphaIndex];
                    }

                    output[outIndex + 3] = alpha;
                }
            }
        }

        private static void DecodeColorBlock(ushort c0, ushort c1, uint bits, Span<byte> output)
        {
            Span<byte> colorTable = stackalloc byte[16];

            Decode565(c0, colorTable.Slice(0, 4));
            Decode565(c1, colorTable.Slice(4, 4));

            if (c0 > c1)
            {
                for (int i = 0; i < 3; i++)
                {
                    colorTable[8 + i] = (byte)((2 * colorTable[i] + colorTable[4 + i]) / 3);
                    colorTable[12 + i] = (byte)((colorTable[i] + 2 * colorTable[4 + i]) / 3);
                }

                colorTable[11] = 255;
                colorTable[15] = 255;
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    colorTable[8 + i] = (byte)((colorTable[i] + colorTable[4 + i]) / 2);
                    colorTable[12 + i] = 0;
                }

                colorTable[11] = 255;
                colorTable[15] = 0;
            }

            for (int i = 0; i < 16; i++)
            {
                int index = (int)(bits >> 2 * i & 0x3);

                output[i * 4 + 0] = colorTable[index * 4 + 0];
                output[i * 4 + 1] = colorTable[index * 4 + 1];
                output[i * 4 + 2] = colorTable[index * 4 + 2];
                output[i * 4 + 3] = colorTable[index * 4 + 3];
            }
        }

        private static void Decode565(ushort color, Span<byte> output)
        {
            byte r = (byte)((color >> 11 & 0x1F) * 255 / 31);
            byte g = (byte)((color >> 5 & 0x3F) * 255 / 63);
            byte b = (byte)((color & 0x1F) * 255 / 31);

            // BGRA for WPF
            output[0] = b;
            output[1] = g;
            output[2] = r;
            output[3] = 255;
        }
    }
}