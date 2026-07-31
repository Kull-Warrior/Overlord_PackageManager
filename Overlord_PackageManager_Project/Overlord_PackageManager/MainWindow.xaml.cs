using Microsoft.Win32;
using Overlord_PackageManager.resources.Data.DataTypes;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Audio;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Images.DDS;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Images.ReflectionCubeMap;
using Overlord_PackageManager.resources.Data.EntryTypes.Asset.Mesh;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.CountedList;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawArray;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.RawList;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.Scalar;
using Overlord_PackageManager.resources.Data.EntryTypes.Leaf.VariableWidth;
using Overlord_PackageManager.resources.Data.EntryTypes.Lua;
using Overlord_PackageManager.resources.Data.EntryTypes.XML;
using Overlord_PackageManager.resources.Data.Files.OMP;
using Overlord_PackageManager.resources.Data.Files.RPK;
using Overlord_PackageManager.resources.GUI;
using Overlord_PackageManager.resources.GUI.EntryEditor.Asset.Audio;
using Overlord_PackageManager.resources.GUI.EntryEditor.Asset.Images.DDS;
using Overlord_PackageManager.resources.GUI.EntryEditor.Asset.Images.ReflectionCubeMap;
using Overlord_PackageManager.resources.GUI.EntryEditor.Asset.Lua;
using Overlord_PackageManager.resources.GUI.EntryEditor.Asset.Mesh;
using Overlord_PackageManager.resources.GUI.EntryEditor.Leaf;
using Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.RawArray;
using Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.RawList;
using Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.Scalar;
using Overlord_PackageManager.resources.GUI.EntryEditor.Leaf.VariableWidth;
using Overlord_PackageManager.resources.GUI.EntryEditor.XML;
using Overlord_PackageManager.resources.GUI.ObservableWrappers;
using System.IO;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;

namespace Overlord_PackageManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ResourcePackFile resourceFile;
        OMPFile mapFile;

        string filePath = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Browse_File_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "RPK Files (*.rpk)|*.rpk|" +
                "PRP Files (*.prp)|*.prp|" +
                "PSP Files (*.psp)|*.psp|" +
                "PVP Files (*.pvp)|*.pvp|" +
                "OMP Files (*.omp)|*.omp|" +
                "All files (*.*)| *.*";
            openFileDialog.FilterIndex = 6;
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
            }

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                MessageBox.Show("Please select a valid resource file.");
                return;
            }

            if (filePath.ToString().ToLower().EndsWith(".omp"))
            {
                mapFile = new OMPFile();
                mapFile.Parse(filePath);
                treeView.Items.Clear();
                treeView.Items.Add(RefTableTreeBuilder.BuildFileRoot(openFileDialog.SafeFileName, (mapFile.Body.Info.Table, "Map Info Root"), (mapFile.Body.Data.Table, "Map Data Root")));
                RefTableTreeBuilder.AttachDeleteKeyHandler(treeView);
            }
            else
            {
                resourceFile = new ResourcePackFile();
                resourceFile.Read(filePath);
                treeView.Items.Clear();
                treeView.Items.Add(RefTableTreeBuilder.BuildFileRoot(openFileDialog.SafeFileName, (resourceFile.Body.Data.Table, "Resourcepack Data Root")));
                RefTableTreeBuilder.AttachDeleteKeyHandler(treeView);
            }
        }

        private void ExportAssets(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                MessageBox.Show("Please select a valid resource file.");
                return;
            }

            DirectoryInfo dirInfo = new DirectoryInfo(filePath);
            string parentDir = dirInfo.Parent.FullName + "\\";
            string dirName = Path.GetFileNameWithoutExtension(filePath);

            if (filePath.EndsWith(".omp"))
            {
                //mapFile.WriteAllAssetsToFile(parentDir + dirName);
            }
            else
            {
                resourceFile.WriteAllAssetsToFile(parentDir + dirName);
            }
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treeView.SelectedItem is TreeViewItem item)
            {
                ShowEditor(item.Tag);
            }
        }

        private void ShowEditor(object obj)
        {
            EditorHost.Content = null;

            switch (obj)
            {
                case ScalarEntry<BonePosition> bonePositionEntry:
                    ObservableBonePosition wrapperBonePosition = new ObservableBonePosition(bonePositionEntry.Value);
                    wrapperBonePosition.PropertyChanged += (s, e) => bonePositionEntry.Value = wrapperBonePosition.Value;
                    EditorHost.Content = new BonePositionEditor(wrapperBonePosition);
                    break;
                case ScalarEntry<BoneRotation> boneRotationEntry:
                    ObservableBoneRotation wrapperBoneRotation = new ObservableBoneRotation(boneRotationEntry.Value);
                    wrapperBoneRotation.PropertyChanged += (s, e) => boneRotationEntry.Value = wrapperBoneRotation.Value;
                    EditorHost.Content = new BoneRotationEditor(wrapperBoneRotation);
                    break;
                case ScalarEntry<BoneScale> boneScaleEntry:
                    ObservableBoneScale wrapperBoneScale = new ObservableBoneScale(boneScaleEntry.Value);
                    wrapperBoneScale.PropertyChanged += (s, e) => boneScaleEntry.Value = wrapperBoneScale.Value;
                    EditorHost.Content = new BoneScaleEditor(wrapperBoneScale);
                    break;
                case ScalarEntry<bool> boolEntry:
                    ObservableValue<bool> wrapperBool = new ObservableValue<bool>(boolEntry.Value);
                    wrapperBool.PropertyChanged += (s, e) => boolEntry.Value = wrapperBool.Value;
                    EditorHost.Content = new BoolEditor(wrapperBool);
                    break;
                case ScalarEntry<byte> byteEntry:
                    ObservableValue<byte> wrapperByte = new ObservableValue<byte>(byteEntry.Value);
                    wrapperByte.PropertyChanged += (s, e) => byteEntry.Value = wrapperByte.Value;
                    EditorHost.Content = new ByteEditor(wrapperByte);
                    break;
                case ScalarEntry<char> charEntry:
                    ObservableValue<char> wrapperChar = new ObservableValue<char>(charEntry.Value);
                    wrapperChar.PropertyChanged += (s, e) => charEntry.Value = wrapperChar.Value;
                    EditorHost.Content = new CharEditor(wrapperChar);
                    break;
                case ScalarEntry<double> doubleEntry:
                    ObservableValue<double> wrapperDouble = new ObservableValue<double>(doubleEntry.Value);
                    wrapperDouble.PropertyChanged += (s, e) => doubleEntry.Value = wrapperDouble.Value;
                    EditorHost.Content = new DoubleEditor(wrapperDouble);
                    break;
                case ScalarEntry<float> floatEntry:
                    ObservableValue<float> wrapperFloat = new ObservableValue<float>(floatEntry.Value);
                    wrapperFloat.PropertyChanged += (s, e) => floatEntry.Value = wrapperFloat.Value;
                    EditorHost.Content = new FloatEditor(wrapperFloat);
                    break;
                case ScalarEntry<short> int16Entry:
                    ObservableValue<short> wrapperInt16 = new ObservableValue<short>(int16Entry.Value);
                    wrapperInt16.PropertyChanged += (s, e) => int16Entry.Value = wrapperInt16.Value;
                    EditorHost.Content = new Int16Editor(wrapperInt16);
                    break;
                case ScalarEntry<int> int32Entry:
                    ObservableValue<int> wrapperInt32 = new ObservableValue<int>(int32Entry.Value);
                    wrapperInt32.PropertyChanged += (s, e) => int32Entry.Value = wrapperInt32.Value;
                    EditorHost.Content = new Int32Editor(wrapperInt32);
                    break;
                case ScalarEntry<long> int64Entry:
                    ObservableValue<long> wrapperInt64 = new ObservableValue<long>(int64Entry.Value);
                    wrapperInt64.PropertyChanged += (s, e) => int64Entry.Value = wrapperInt64.Value;
                    EditorHost.Content = new Int64Editor(wrapperInt64);
                    break;
                case ScalarEntry<Matrix3x3> matrix3x3Entry:
                    ObservableMatrix3x3 wrapperMatrix3x3 = new ObservableMatrix3x3(matrix3x3Entry.Value);
                    wrapperMatrix3x3.PropertyChanged += (s, e) => matrix3x3Entry.Value = wrapperMatrix3x3.Matrix;
                    EditorHost.Content = new Matrix3x3Editor(wrapperMatrix3x3);
                    break;
                case ScalarEntry<Matrix4x4> matrix4x4Entry:
                    ObservableMatrix4x4 wrapperMatrix4x4 = new ObservableMatrix4x4(matrix4x4Entry.Value);
                    wrapperMatrix4x4.PropertyChanged += (s, e) => matrix4x4Entry.Value = wrapperMatrix4x4.Matrix;
                    EditorHost.Content = new Matrix4x4Editor(wrapperMatrix4x4);
                    break;
                case ScalarEntry<MeshBoneShape> meshBoneShapeEntry:
                    ObservableMeshBoneShape observableMeshBoneShape = new ObservableMeshBoneShape(meshBoneShapeEntry.Value);
                    observableMeshBoneShape.PropertyChanged += (s, e) => meshBoneShapeEntry.Value = observableMeshBoneShape.Value;
                    EditorHost.Content = new MeshBoneShapeEditor(observableMeshBoneShape);
                    break;
                case ScalarEntry<ObjectBone> objectBoneEntry:
                    ObservableObjectBone observableObjectBone = new ObservableObjectBone(objectBoneEntry.Value);
                    observableObjectBone.PropertyChanged += (s, e) => objectBoneEntry.Value = observableObjectBone.Value;
                    EditorHost.Content = new ObjectBoneEditor(observableObjectBone);
                    break;
                case ScalarEntry<Quaternion> quaternionEntry:
                    ObservableQuaternion observableQuaternion = new ObservableQuaternion(quaternionEntry.Value);
                    observableQuaternion.PropertyChanged += (s, e) => quaternionEntry.Value = observableQuaternion.Value;
                    EditorHost.Content = new QuaternionEditor(observableQuaternion);
                    break;
                case ScalarEntry<RawMeshClusterData> rawMeshClusterDataEntry:
                    ObservableRawMeshClusterData observableRawMeshClusterData = new ObservableRawMeshClusterData(rawMeshClusterDataEntry.Value);
                    observableRawMeshClusterData.PropertyChanged += (s, e) => rawMeshClusterDataEntry.Value = observableRawMeshClusterData.Value;
                    EditorHost.Content = new RawMeshClusterDataEditor(observableRawMeshClusterData);
                    break;
                case ScalarEntry<Transform> transformEntry:
                    ObservableTransform observableTransform = new ObservableTransform(transformEntry.Value);
                    observableTransform.PropertyChanged += (s, e) => transformEntry.Value = observableTransform.Value;
                    EditorHost.Content = new TransformEditor(observableTransform);
                    break;
                case ScalarEntry<ushort> uint16Entry:
                    ObservableValue<ushort> wrapperUInt16 = new ObservableValue<ushort>(uint16Entry.Value);
                    wrapperUInt16.PropertyChanged += (s, e) => uint16Entry.Value = wrapperUInt16.Value;
                    EditorHost.Content = new UInt16Editor(wrapperUInt16);
                    break;
                case ScalarEntry<uint> uint32Entry:
                    ObservableValue<uint> wrapperUInt32 = new ObservableValue<uint>(uint32Entry.Value);
                    wrapperUInt32.PropertyChanged += (s, e) => uint32Entry.Value = wrapperUInt32.Value;
                    EditorHost.Content = new UInt32Editor(wrapperUInt32);
                    break;
                case ScalarEntry<ulong> uint64Entry:
                    ObservableValue<ulong> wrapperUInt64 = new ObservableValue<ulong>(uint64Entry.Value);
                    wrapperUInt64.PropertyChanged += (s, e) => uint64Entry.Value = wrapperUInt64.Value;
                    EditorHost.Content = new UInt64Editor(wrapperUInt64);
                    break;
                case ScalarEntry<Vector3> vector3Entry:
                    ObservableVector3 observableVector3 = new ObservableVector3(vector3Entry.Value);
                    observableVector3.PropertyChanged += (s, e) => vector3Entry.Value = observableVector3.Value;
                    EditorHost.Content = new Vector3Editor(observableVector3);
                    break;
                case ScalarEntry<Vector4> vector4Entry:
                    ObservableVector4 observableVector4 = new ObservableVector4(vector4Entry.Value);
                    observableVector4.PropertyChanged += (s, e) => vector4Entry.Value = observableVector4.Value;
                    EditorHost.Content = new Vector4Editor(observableVector4);
                    break;
                case ScalarEntry<VertexAttribute> vertexAttributeEntry:
                    ObservableVertexAttribute observableVertexAttribute = new ObservableVertexAttribute(vertexAttributeEntry.Value);
                    observableVertexAttribute.PropertyChanged += (s, e) => vertexAttributeEntry.Value = observableVertexAttribute.Value;
                    EditorHost.Content = new VertexAttributeEditor(observableVertexAttribute);
                    break;
                case RawArrayEntry<BonePosition> rawBonePositionArrayEntry:
                    ObservableValue<BonePosition[]> observableRawBonePositionArray = new ObservableValue<BonePosition[]>(rawBonePositionArrayEntry.Value);
                    observableRawBonePositionArray.PropertyChanged += (s, e) => rawBonePositionArrayEntry.Value = observableRawBonePositionArray.Value;
                    EditorHost.Content = new BonePositionArrayEditor(observableRawBonePositionArray);
                    break;
                case CountedArrayEntry<BonePosition> countedBonePositionArrayEntry:
                    ObservableValue<BonePosition[]> observableCountedBonePositionArray = new ObservableValue<BonePosition[]>(countedBonePositionArrayEntry.Value);
                    observableCountedBonePositionArray.PropertyChanged += (s, e) => countedBonePositionArrayEntry.Value = observableCountedBonePositionArray.Value;
                    EditorHost.Content = new BonePositionArrayEditor(observableCountedBonePositionArray);
                    break;
                case RawArrayEntry<BoneRotation> rawBoneRotationArrayEntry:
                    ObservableValue<BoneRotation[]> observableRawBoneRotationArray = new ObservableValue<BoneRotation[]>(rawBoneRotationArrayEntry.Value);
                    observableRawBoneRotationArray.PropertyChanged += (s, e) => rawBoneRotationArrayEntry.Value = observableRawBoneRotationArray.Value;
                    EditorHost.Content = new BoneRotationArrayEditor(observableRawBoneRotationArray);
                    break;
                case CountedArrayEntry<BoneRotation> countedBoneRotationArrayEntry:
                    ObservableValue<BoneRotation[]> observableCountedBoneRotationArray = new ObservableValue<BoneRotation[]>(countedBoneRotationArrayEntry.Value);
                    observableCountedBoneRotationArray.PropertyChanged += (s, e) => countedBoneRotationArrayEntry.Value = observableCountedBoneRotationArray.Value;
                    EditorHost.Content = new BoneRotationArrayEditor(observableCountedBoneRotationArray);
                    break;
                case RawArrayEntry<BoneScale> rawBoneScaleArrayEntry:
                    ObservableValue<BoneScale[]> observableRawBoneScaleArray = new ObservableValue<BoneScale[]>(rawBoneScaleArrayEntry.Value);
                    observableRawBoneScaleArray.PropertyChanged += (s, e) => rawBoneScaleArrayEntry.Value = observableRawBoneScaleArray.Value;
                    EditorHost.Content = new BoneScaleArrayEditor(observableRawBoneScaleArray);
                    break;
                case CountedArrayEntry<BoneScale> countedBoneScaleArrayEntry:
                    ObservableValue<BoneScale[]> observableCountedBoneScaleArray = new ObservableValue<BoneScale[]>(countedBoneScaleArrayEntry.Value);
                    observableCountedBoneScaleArray.PropertyChanged += (s, e) => countedBoneScaleArrayEntry.Value = observableCountedBoneScaleArray.Value;
                    EditorHost.Content = new BoneScaleArrayEditor(observableCountedBoneScaleArray);
                    break;
                case RawArrayEntry<bool> rawBooleanArrayEntry:
                    ObservableValue<bool[]> observableRawBooleanArray = new ObservableValue<bool[]>(rawBooleanArrayEntry.Value);
                    observableRawBooleanArray.PropertyChanged += (s, e) => rawBooleanArrayEntry.Value = observableRawBooleanArray.Value;
                    EditorHost.Content = new BoolArrayEditor(observableRawBooleanArray);
                    break;
                case CountedArrayEntry<bool> countedBooleanArrayEntry:
                    ObservableValue<bool[]> observableCountedBooleanArray = new ObservableValue<bool[]>(countedBooleanArrayEntry.Value);
                    observableCountedBooleanArray.PropertyChanged += (s, e) => countedBooleanArrayEntry.Value = observableCountedBooleanArray.Value;
                    EditorHost.Content = new BoolArrayEditor(observableCountedBooleanArray);
                    break;
                case RawArrayEntry<byte> rawByteArrayEntry:
                    ObservableValue<byte[]> observableRawByteArray = new ObservableValue<byte[]>(rawByteArrayEntry.Value);
                    observableRawByteArray.PropertyChanged += (s, e) => rawByteArrayEntry.Value = observableRawByteArray.Value;
                    EditorHost.Content = new ByteArrayEntryEditor(observableRawByteArray);
                    break;
                case CountedArrayEntry<byte> countedByteArrayEntry:
                    ObservableValue<byte[]> observableCountedByteArray = new ObservableValue<byte[]>(countedByteArrayEntry.Value);
                    observableCountedByteArray.PropertyChanged += (s, e) => countedByteArrayEntry.Value = observableCountedByteArray.Value;
                    EditorHost.Content = new ByteArrayEntryEditor(observableCountedByteArray);
                    break;
                case RawArrayEntry<char> rawCharArrayEntry:
                    ObservableValue<char[]> observableRawCharArray = new ObservableValue<char[]>(rawCharArrayEntry.Value);
                    observableRawCharArray.PropertyChanged += (s, e) => rawCharArrayEntry.Value = observableRawCharArray.Value;
                    EditorHost.Content = new StringEditor(observableRawCharArray);
                    break;
                case CountedArrayEntry<char> charCountedArray:
                    ObservableValue<char[]> observableCountedCharArray = new ObservableValue<char[]>(charCountedArray.Value);
                    observableCountedCharArray.PropertyChanged += (s, e) => charCountedArray.Value = observableCountedCharArray.Value;
                    EditorHost.Content = new StringEditor(observableCountedCharArray);
                    break;
                case RawArrayEntry<double> doubleArrayEntry:
                    ObservableValue<double[]> observableRawDoubleArray = new ObservableValue<double[]>(doubleArrayEntry.Value);
                    observableRawDoubleArray.PropertyChanged += (s, e) => doubleArrayEntry.Value = observableRawDoubleArray.Value;
                    EditorHost.Content = new DoubleArrayEditor(observableRawDoubleArray);
                    break;
                case CountedArrayEntry<double> countedDoubleArrayEntry:
                    ObservableValue<double[]> observableCountedDoubleArray = new ObservableValue<double[]>(countedDoubleArrayEntry.Value);
                    observableCountedDoubleArray.PropertyChanged += (s, e) => countedDoubleArrayEntry.Value = observableCountedDoubleArray.Value;
                    EditorHost.Content = new DoubleArrayEditor(observableCountedDoubleArray);
                    break;
                case RawArrayEntry<float> floatArrayEntry:
                    ObservableValue<float[]> observableRawFloatArray = new ObservableValue<float[]>(floatArrayEntry.Value);
                    observableRawFloatArray.PropertyChanged += (s, e) => floatArrayEntry.Value = observableRawFloatArray.Value;
                    EditorHost.Content = new FloatArrayEditor(observableRawFloatArray);
                    break;
                case CountedArrayEntry<float> countedFloatArrayEntry:
                    ObservableValue<float[]> observableCountedFloatArray = new ObservableValue<float[]>(countedFloatArrayEntry.Value);
                    observableCountedFloatArray.PropertyChanged += (s, e) => countedFloatArrayEntry.Value = observableCountedFloatArray.Value;
                    EditorHost.Content = new FloatArrayEditor(observableCountedFloatArray);
                    break;
                case RawArrayEntry<short> int16ArrayEntry:
                    ObservableValue<short[]> observableRawInt16Array = new ObservableValue<short[]>(int16ArrayEntry.Value);
                    observableRawInt16Array.PropertyChanged += (s, e) => int16ArrayEntry.Value = observableRawInt16Array.Value;
                    EditorHost.Content = new Int16ArrayEditor(observableRawInt16Array);
                    break;
                case CountedArrayEntry<short> countedInt16ArrayEntry:
                    ObservableValue<short[]> observableCountedInt16Array = new ObservableValue<short[]>(countedInt16ArrayEntry.Value);
                    observableCountedInt16Array.PropertyChanged += (s, e) => countedInt16ArrayEntry.Value = observableCountedInt16Array.Value;
                    EditorHost.Content = new Int16ArrayEditor(observableCountedInt16Array);
                    break;
                case RawArrayEntry<int> uint32ArrayEntry:
                    ObservableValue<int[]> observableRawInt32Array = new ObservableValue<int[]>(uint32ArrayEntry.Value);
                    observableRawInt32Array.PropertyChanged += (s, e) => uint32ArrayEntry.Value = observableRawInt32Array.Value;
                    EditorHost.Content = new Int32ArrayEditor(observableRawInt32Array);
                    break;
                case CountedArrayEntry<int> countedInt32ArrayEntry:
                    ObservableValue<int[]> observableCountedInt32Array = new ObservableValue<int[]>(countedInt32ArrayEntry.Value);
                    observableCountedInt32Array.PropertyChanged += (s, e) => countedInt32ArrayEntry.Value = observableCountedInt32Array.Value;
                    EditorHost.Content = new Int32ArrayEditor(observableCountedInt32Array);
                    break;
                case RawArrayEntry<long> int64ArrayEntry:
                    ObservableValue<long[]> observableRawInt64Array = new ObservableValue<long[]>(int64ArrayEntry.Value);
                    observableRawInt64Array.PropertyChanged += (s, e) => int64ArrayEntry.Value = observableRawInt64Array.Value;
                    EditorHost.Content = new Int64ArrayEditor(observableRawInt64Array);
                    break;
                case CountedArrayEntry<long> countedInt64ArrayEntry:
                    ObservableValue<long[]> observableCountedInt64Array = new ObservableValue<long[]>(countedInt64ArrayEntry.Value);
                    observableCountedInt64Array.PropertyChanged += (s, e) => countedInt64ArrayEntry.Value = observableCountedInt64Array.Value;
                    EditorHost.Content = new Int64ArrayEditor(observableCountedInt64Array);
                    break;
                case RawArrayEntry<Matrix3x3> matrices3x3ArrayEntry:
                    ObservableValue<Matrix3x3[]> observableMatrices3x3Array = new ObservableValue<Matrix3x3[]>(matrices3x3ArrayEntry.Value);
                    observableMatrices3x3Array.PropertyChanged += (s, e) => matrices3x3ArrayEntry.Value = observableMatrices3x3Array.Value;
                    EditorHost.Content = new Matrix3x3ArrayEditor(observableMatrices3x3Array);
                    break;
                case CountedArrayEntry<Matrix3x3> countedMatrices3x3ArrayEntry:
                    ObservableValue<Matrix3x3[]> observableCountedMatrices3x3Array = new ObservableValue<Matrix3x3[]>(countedMatrices3x3ArrayEntry.Value);
                    observableCountedMatrices3x3Array.PropertyChanged += (s, e) => countedMatrices3x3ArrayEntry.Value = observableCountedMatrices3x3Array.Value;
                    EditorHost.Content = new Matrix3x3ArrayEditor(observableCountedMatrices3x3Array);
                    break;
                case RawArrayEntry<Matrix4x4> matrices4x4ArrayEntry:
                    ObservableValue<Matrix4x4[]> observableMatrices4x4Array = new ObservableValue<Matrix4x4[]>(matrices4x4ArrayEntry.Value);
                    observableMatrices4x4Array.PropertyChanged += (s, e) => matrices4x4ArrayEntry.Value = observableMatrices4x4Array.Value;
                    EditorHost.Content = new Matrix4x4ArrayEditor(observableMatrices4x4Array);
                    break;
                case CountedArrayEntry<Matrix4x4> countedMatrices4x4ArrayEntry:
                    ObservableValue<Matrix4x4[]> observableCountedMatrices4x4Array = new ObservableValue<Matrix4x4[]>(countedMatrices4x4ArrayEntry.Value);
                    observableCountedMatrices4x4Array.PropertyChanged += (s, e) => countedMatrices4x4ArrayEntry.Value = observableCountedMatrices4x4Array.Value;
                    EditorHost.Content = new Matrix4x4ArrayEditor(observableCountedMatrices4x4Array);
                    break;
                case RawArrayEntry<MeshBoneShape> meshBoneShapeArrayEntry:
                    ObservableValue<MeshBoneShape[]> observableMeshBoneShapeArray = new ObservableValue<MeshBoneShape[]>(meshBoneShapeArrayEntry.Value);
                    observableMeshBoneShapeArray.PropertyChanged += (s, e) => meshBoneShapeArrayEntry.Value = observableMeshBoneShapeArray.Value;
                    EditorHost.Content = new MeshBoneShapeArrayEditor(observableMeshBoneShapeArray);
                    break;
                case CountedArrayEntry<MeshBoneShape> countedMeshBoneShapeArrayEntry:
                    ObservableValue<MeshBoneShape[]> observableCountedMeshBoneShapeArray = new ObservableValue<MeshBoneShape[]>(countedMeshBoneShapeArrayEntry.Value);
                    observableCountedMeshBoneShapeArray.PropertyChanged += (s, e) => countedMeshBoneShapeArrayEntry.Value = observableCountedMeshBoneShapeArray.Value;
                    EditorHost.Content = new MeshBoneShapeArrayEditor(observableCountedMeshBoneShapeArray);
                    break;
                case RawArrayEntry<ObjectBone> objectBoneArrayEntry:
                    ObservableValue<ObjectBone[]> observableObjectBoneArray = new ObservableValue<ObjectBone[]>(objectBoneArrayEntry.Value);
                    observableObjectBoneArray.PropertyChanged += (s, e) => objectBoneArrayEntry.Value = observableObjectBoneArray.Value;
                    EditorHost.Content = new ObjectBoneArrayEditor(observableObjectBoneArray);
                    break;
                case CountedArrayEntry<ObjectBone> countedObjectBoneArrayEntry:
                    ObservableValue<ObjectBone[]> observableCountedObjectBoneArray = new ObservableValue<ObjectBone[]>(countedObjectBoneArrayEntry.Value);
                    observableCountedObjectBoneArray.PropertyChanged += (s, e) => countedObjectBoneArrayEntry.Value = observableCountedObjectBoneArray.Value;
                    EditorHost.Content = new ObjectBoneArrayEditor(observableCountedObjectBoneArray);
                    break;
                case RawArrayEntry<Quaternion> quaternionArrayEntry:
                    ObservableValue<Quaternion[]> observableQuaternionArray = new ObservableValue<Quaternion[]>(quaternionArrayEntry.Value);
                    observableQuaternionArray.PropertyChanged += (s, e) => quaternionArrayEntry.Value = observableQuaternionArray.Value;
                    EditorHost.Content = new QuaternionArrayEditor(observableQuaternionArray);
                    break;
                case CountedArrayEntry<Quaternion> countedQuaternionArrayEntry:
                    ObservableValue<Quaternion[]> observableCountedQuaternionArray = new ObservableValue<Quaternion[]>(countedQuaternionArrayEntry.Value);
                    observableCountedQuaternionArray.PropertyChanged += (s, e) => countedQuaternionArrayEntry.Value = observableCountedQuaternionArray.Value;
                    EditorHost.Content = new QuaternionArrayEditor(observableCountedQuaternionArray);
                    break;
                case RawArrayEntry<RawMeshClusterData> rawMeshClusterDataArrayEntry:
                    ObservableValue<RawMeshClusterData[]> observableRawMeshClusterDataArray = new ObservableValue<RawMeshClusterData[]>(rawMeshClusterDataArrayEntry.Value);
                    observableRawMeshClusterDataArray.PropertyChanged += (s, e) => rawMeshClusterDataArrayEntry.Value = observableRawMeshClusterDataArray.Value;
                    EditorHost.Content = new RawMeshClusterDataArrayEntryEditor(observableRawMeshClusterDataArray);
                    break;
                case CountedArrayEntry<RawMeshClusterData> countedMeshClusterDataArrayEntry:
                    ObservableValue<RawMeshClusterData[]> observableCountedMeshClusterDataArray = new ObservableValue<RawMeshClusterData[]>(countedMeshClusterDataArrayEntry.Value);
                    observableCountedMeshClusterDataArray.PropertyChanged += (s, e) => countedMeshClusterDataArrayEntry.Value = observableCountedMeshClusterDataArray.Value;
                    EditorHost.Content = new RawMeshClusterDataArrayEntryEditor(observableCountedMeshClusterDataArray);
                    break;
                case RawArrayEntry<Transform> transformArrayEntry:
                    ObservableValue<Transform[]> observableTransformArray = new ObservableValue<Transform[]>(transformArrayEntry.Value);
                    observableTransformArray.PropertyChanged += (s, e) => transformArrayEntry.Value = observableTransformArray.Value;
                    EditorHost.Content = new TransformArrayEditor(observableTransformArray);
                    break;
                case CountedArrayEntry<Transform> countedTransformArrayEntry:
                    ObservableValue<Transform[]> observableCountedTransformArray = new ObservableValue<Transform[]>(countedTransformArrayEntry.Value);
                    observableCountedTransformArray.PropertyChanged += (s, e) => countedTransformArrayEntry.Value = observableCountedTransformArray.Value;
                    EditorHost.Content = new TransformArrayEditor(observableCountedTransformArray);
                    break;
                case RawArrayEntry<ushort> uint16ArrayEntry:
                    ObservableValue<ushort[]> observableRawUInt16Array = new ObservableValue<ushort[]>(uint16ArrayEntry.Value);
                    observableRawUInt16Array.PropertyChanged += (s, e) => uint16ArrayEntry.Value = observableRawUInt16Array.Value;
                    EditorHost.Content = new UInt16ArrayEditor(observableRawUInt16Array);
                    break;
                case CountedArrayEntry<ushort> countedUInt16ArrayEntry:
                    ObservableValue<ushort[]> observableCountedUInt16Array = new ObservableValue<ushort[]>(countedUInt16ArrayEntry.Value);
                    observableCountedUInt16Array.PropertyChanged += (s, e) => countedUInt16ArrayEntry.Value = observableCountedUInt16Array.Value;
                    EditorHost.Content = new UInt16ArrayEditor(observableCountedUInt16Array);
                    break;
                case RawArrayEntry<uint> uint32ArrayEntry:
                    ObservableValue<uint[]> observableRawUInt32Array = new ObservableValue<uint[]>(uint32ArrayEntry.Value);
                    observableRawUInt32Array.PropertyChanged += (s, e) => uint32ArrayEntry.Value = observableRawUInt32Array.Value;
                    EditorHost.Content = new UInt32ArrayEditor(observableRawUInt32Array);
                    break;
                case CountedArrayEntry<uint> countedUInt32ArrayEntry:
                    ObservableValue<uint[]> observableCountedUInt32Array = new ObservableValue<uint[]>(countedUInt32ArrayEntry.Value);
                    observableCountedUInt32Array.PropertyChanged += (s, e) => countedUInt32ArrayEntry.Value = observableCountedUInt32Array.Value;
                    EditorHost.Content = new UInt32ArrayEditor(observableCountedUInt32Array);
                    break;
                case RawArrayEntry<ulong> uint64ArrayEntry:
                    ObservableValue<ulong[]> observableRawUInt64Array = new ObservableValue<ulong[]>(uint64ArrayEntry.Value);
                    observableRawUInt64Array.PropertyChanged += (s, e) => uint64ArrayEntry.Value = observableRawUInt64Array.Value;
                    EditorHost.Content = new UInt64ArrayEditor(observableRawUInt64Array);
                    break;
                case CountedArrayEntry<ulong> countedUInt64ArrayEntry:
                    ObservableValue<ulong[]> observableCountedUInt64Array = new ObservableValue<ulong[]>(countedUInt64ArrayEntry.Value);
                    observableCountedUInt64Array.PropertyChanged += (s, e) => countedUInt64ArrayEntry.Value = observableCountedUInt64Array.Value;
                    EditorHost.Content = new UInt64ArrayEditor(observableCountedUInt64Array);
                    break;
                case RawArrayEntry<Vector3> vector3ArrayEntry:
                    ObservableValue<Vector3[]> observableRawVector3Array = new ObservableValue<Vector3[]>(vector3ArrayEntry.Value);
                    observableRawVector3Array.PropertyChanged += (s, e) => vector3ArrayEntry.Value = observableRawVector3Array.Value;
                    EditorHost.Content = new Vector3ArrayEditor(observableRawVector3Array);
                    break;
                case CountedArrayEntry<Vector3> countedVector3ArrayEntry:
                    ObservableValue<Vector3[]> observableCountedVector3Array = new ObservableValue<Vector3[]>(countedVector3ArrayEntry.Value);
                    observableCountedVector3Array.PropertyChanged += (s, e) => countedVector3ArrayEntry.Value = observableCountedVector3Array.Value;
                    EditorHost.Content = new Vector3ArrayEditor(observableCountedVector3Array);
                    break;
                case RawArrayEntry<Vector4> vector4ArrayEntry:
                    ObservableValue<Vector4[]> observableRawVector4Array = new ObservableValue<Vector4[]>(vector4ArrayEntry.Value);
                    observableRawVector4Array.PropertyChanged += (s, e) => vector4ArrayEntry.Value = observableRawVector4Array.Value;
                    EditorHost.Content = new Vector4ArrayEditor(observableRawVector4Array);
                    break;
                case CountedArrayEntry<Vector4> countedVector4ArrayEntry:
                    ObservableValue<Vector4[]> observableCountedVector4Array = new ObservableValue<Vector4[]>(countedVector4ArrayEntry.Value);
                    observableCountedVector4Array.PropertyChanged += (s, e) => countedVector4ArrayEntry.Value = observableCountedVector4Array.Value;
                    EditorHost.Content = new Vector4ArrayEditor(observableCountedVector4Array);
                    break;
                case RawArrayEntry<VertexAttribute> vertexAttributeArrayEntry:
                    ObservableValue<VertexAttribute[]> observableRawVertexAttributeArray = new ObservableValue<VertexAttribute[]>(vertexAttributeArrayEntry.Value);
                    observableRawVertexAttributeArray.PropertyChanged += (s, e) => vertexAttributeArrayEntry.Value = observableRawVertexAttributeArray.Value;
                    EditorHost.Content = new VertexAttributeArrayEditor(observableRawVertexAttributeArray);
                    break;
                case CountedArrayEntry<VertexAttribute> countedVertexAttributeArrayEntry:
                    ObservableValue<VertexAttribute[]> observableCountedVertexAttributeArray = new ObservableValue<VertexAttribute[]>(countedVertexAttributeArrayEntry.Value);
                    observableCountedVertexAttributeArray.PropertyChanged += (s, e) => countedVertexAttributeArrayEntry.Value = observableCountedVertexAttributeArray.Value;
                    EditorHost.Content = new VertexAttributeArrayEditor(observableCountedVertexAttributeArray);
                    break;
                case MeshData meshData:
                    EditorHost.Content = new MeshDataEditor(meshData);
                    break;
                case RawListEntry<VertexAttribute> vertexDeclaration:
                    EditorHost.Content = new VertexDeclarationEditor(vertexDeclaration);
                    break;
                case DDSTextures ddsTextures:
                    EditorHost.Content = new DDSTextureEditor(ddsTextures);
                    break;
                case DDSTextureAsset asset:
                    EditorHost.Content = new DDSTextureAssetEditor(asset);
                    break;
                case ReflectionCubeMapAsset reflectionCubeMap:
                    EditorHost.Content = new ReflectionCubeMapEditor(reflectionCubeMap);
                    break;
                case SFXAsset sfxAsset:
                    EditorHost.Content = new SFXAssetEditor(sfxAsset);
                    break;
                case XMLEntry xmlEntry:
                    EditorHost.Content = new XMLAssetEditor(xmlEntry);
                    break;
                case CharListCountedArrayEntry charListCountedArrayEntry:
                    EditorHost.Content = new StringCountedListEntryEditor(charListCountedArrayEntry);
                    break;
                case LuaEntry luaEntry:
                    EditorHost.Content = new LuaEntryEditor(luaEntry);
                    break;
                default:
                    EditorHost.Content = new TextBlock
                    {
                        Text = obj?.ToString() ?? "null"
                    };
                    break;
            }
        }

        private void New(object sender, RoutedEventArgs e)
        {

        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("No file loaded.");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = Path.GetFileName(filePath);
            saveFileDialog.InitialDirectory = Path.GetDirectoryName(filePath);

            saveFileDialog.Filter =
                "RPK Files (*.rpk)|*.rpk|" +
                "PRP Files (*.prp)|*.prp|" +
                "PSP Files (*.psp)|*.psp|" +
                "PVP Files (*.pvp)|*.pvp|" +
                "OMP Files (*.omp)|*.omp|" +
                "All files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() != true)
            {
                return;
            }

            string savePath = saveFileDialog.FileName;

            try
            {
                if (savePath.EndsWith(".omp", StringComparison.OrdinalIgnoreCase))
                {
                    if (mapFile == null)
                    {
                        MessageBox.Show("No OMP file loaded.");
                        return;
                    }

                    mapFile.Write(savePath);
                }
                else
                {
                    if (resourceFile == null)
                    {
                        MessageBox.Show("No resource file loaded.");
                        return;
                    }

                    resourceFile.Write(savePath);
                }

                MessageBox.Show("File saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving file: " + ex.Message);
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}