namespace Overlord_PackageManager.resources.Data.DataTypes
{
    public readonly record struct Matrix3x3(float M11, float M12, float M13, float M21, float M22, float M23, float M31, float M32, float M33)
    {
        // Statische Eigenschaften analog zu Matrix4x4
        public static Matrix3x3 Identity => new(1f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f);

        public bool IsIdentity =>
            M11 == 1f && M12 == 0f && M13 == 0f &&
            M21 == 0f && M22 == 1f && M23 == 0f &&
            M31 == 0f && M32 == 0f && M33 == 1f;

        // Mathematische Operatoren (+, -, *)
        public static Matrix3x3 operator +(Matrix3x3 m1, Matrix3x3 m2) => new(
            m1.M11 + m2.M11, m1.M12 + m2.M12, m1.M13 + m2.M13,
            m1.M21 + m2.M21, m1.M22 + m2.M22, m1.M23 + m2.M23,
            m1.M31 + m2.M31, m1.M32 + m2.M32, m1.M33 + m2.M33
        );

        public static Matrix3x3 operator -(Matrix3x3 m1, Matrix3x3 m2) => new(
            m1.M11 - m2.M11, m1.M12 - m2.M12, m1.M13 - m2.M13,
            m1.M21 - m2.M21, m1.M22 - m2.M22, m1.M23 - m2.M23,
            m1.M31 - m2.M31, m1.M32 - m2.M32, m1.M33 - m2.M33
        );

        // Matrix-Multiplikation
        public static Matrix3x3 operator *(Matrix3x3 m1, Matrix3x3 m2) => new(
            m1.M11 * m2.M11 + m1.M12 * m2.M21 + m1.M13 * m2.M31,
            m1.M11 * m2.M12 + m1.M12 * m2.M22 + m1.M13 * m2.M32,
            m1.M11 * m2.M13 + m1.M12 * m2.M23 + m1.M13 * m2.M33,

            m1.M21 * m2.M11 + m1.M22 * m2.M21 + m1.M23 * m2.M31,
            m1.M21 * m2.M12 + m1.M22 * m2.M22 + m1.M23 * m2.M32,
            m1.M21 * m2.M13 + m1.M22 * m2.M23 + m1.M23 * m2.M33,

            m1.M31 * m2.M11 + m1.M32 * m2.M21 + m1.M33 * m2.M31,
            m1.M31 * m2.M12 + m1.M32 * m2.M22 + m1.M33 * m2.M32,
            m1.M31 * m2.M13 + m1.M32 * m2.M23 + m1.M33 * m2.M33
        );

        // Skalar-Multiplikation
        public static Matrix3x3 operator *(Matrix3x3 matrix, float scalar) => new(
            matrix.M11 * scalar, matrix.M12 * scalar, matrix.M13 * scalar,
            matrix.M21 * scalar, matrix.M22 * scalar, matrix.M23 * scalar,
            matrix.M31 * scalar, matrix.M32 * scalar, matrix.M33 * scalar
        );

        // Standard-Methoden aus Matrix4x4
        public float GetDeterminant() =>
            M11 * (M22 * M33 - M23 * M32) -
            M12 * (M21 * M33 - M23 * M31) +
            M13 * (M21 * M32 - M22 * M31);

        public static Matrix3x3 Transpose(Matrix3x3 matrix) => new(
            matrix.M11, matrix.M21, matrix.M31,
            matrix.M12, matrix.M22, matrix.M32,
            matrix.M13, matrix.M23, matrix.M33
        );
    }
}