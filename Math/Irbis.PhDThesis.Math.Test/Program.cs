using System.Numerics;
using System.Text;

using Irbis.Crypto.Domain;

using Irbis.PhDThesis.Domain.Extensions;
using Irbis.PhDThesis.Math.Domain;
using Irbis.PhDThesis.Math.Domain.Fraction;
using Irbis.PhDThesis.Math.Domain.FractionTree;
using Irbis.PhDThesis.Math.Encryption;
using Irbis.PhDThesis.Math.FractionTree.CalkinWilf;
using Irbis.PhDThesis.Math.FractionTree.SternBrokot;

int GetFractionBitsCount(
    Fraction fraction)
{
    var bitsForNumerator = Math.Ceiling(BigInteger.Log(fraction.Numerator + 1, 2));
    var bitsForDenominator = Math.Ceiling(BigInteger.Log(fraction.Denominator + 1, 2));

    return (int)(bitsForNumerator + bitsForDenominator);
}

void TestBitsCountInBitArray(
    int iterationsCount = 10000)
{
    for (var i = 0; i < 1001; i++)
    {
        var bitsCountExpected = i;
        var bitArray = new BitArray(bitsCountExpected);
        Console.WriteLine($"[{i:0000}] {(bitsCountExpected == bitArray.BitsCount ? "P" : "Not p")}assed: expected - {bitsCountExpected:0000}, actual - {bitArray.BitsCount:0000}");
    }

    for (var i = 0; i < 1001; i++)
    {
        var bitsCountExpected = i;
        var bitArray = new BitArray(Enumerable.Repeat(false, i));
        Console.WriteLine($"[{i:0000}] {(bitsCountExpected == bitArray.BitsCount ? "P" : "Not p")}assed: expected - {bitsCountExpected:0000}, actual - {bitArray.BitsCount:0000}");
    }
}

void TestHomogenousHypergraphConstruction()
{
    var hg1 = new HomogenousHypergraph(3, 2, new HyperEdge[] { new (0, 1), new (0, 2), new(1, 2) });
}

void TestHomogenousHypergraphIteration()
{
    var hg = new HomogenousHypergraph(3, 2, new HyperEdge[] { new (0, 1), new (0, 2), new(1, 2) });
    
    for (var i = 0; i < BigIntegerExtensions.CombinationsCount(hg.VerticesCount, hg.SimplicesDimension); i++)
    {
        Console.WriteLine($"{i}: {{{string.Join(", ", hg.BitIndexToSimplex(i))}}} - {(hg.ContainsSimplex(i) ? string.Empty : "not ")}set");
    }
}

void TestHomogenousHypergraphEtc()
{
    var hg = new HomogenousHypergraph(11, 7, new HyperEdge[] { new (10, 9, 8, 7, 6, 5, 4), new (1, 2, 3, 4, 5, 6, 7) });

    foreach (var simplex in hg)
    {
        Console.WriteLine($"{{{simplex}}}");
    }
}

void TestContinuedFraction(
    params Fraction[] fractions)
{
    foreach (var fraction in fractions)
    {
        var i = 0;

        Console.Write($"{fraction} == [ ");
        var continuedFraction = fraction.ToContinuedFraction();
        using var continuedFractionCoefficientsEnumerator = continuedFraction.GetEnumerator();
        if (continuedFractionCoefficientsEnumerator.MoveNext())
        {
            while (true)
            {
                Console.Write(continuedFractionCoefficientsEnumerator.Current);
                var enumeratorMoveNextSucceeded = continuedFractionCoefficientsEnumerator.MoveNext();

                if (i == 0)
                {
                    i = 1;
                    Console.Write("; ");
                    continue;
                }

                if (!enumeratorMoveNextSucceeded)
                {
                    break;
                }

                Console.Write(", ");
            }
        }

        Console.WriteLine($" ] == {continuedFraction.ToFraction()}");
    }
}

void TestFractionTrees(
    int pathLengthMinBound = 1000,
    int pathLengthMaxBound = 2000,
    int? randomSource = null)
{
    randomSource ??= new Random().Next();
    var random = new Random(randomSource.Value);
    Console.WriteLine($"Seed: {randomSource.Value}");
    
    var pathLength = random.Next(pathLengthMinBound, pathLengthMaxBound);
    Console.WriteLine($"Bits count: {pathLength}");
    
    var bits = Enumerable
        .Repeat(0, pathLength)
        .Select(_ => random.Next(2) == 0)
        .ToArray();
    var targetPath = new BitArray(bits);
    
    //Console.WriteLine("Initial path: ");
    //foreach (var pathPart in targetPath)
    //{
    //    Console.Write($"{pathPart} ");
    //}
    //Console.WriteLine();
    
    var trees = new IFractionTree[] { new SternBrokotTree(), new CalkinWilfTree() }
        .Zip(new [] { "Stern-Brokot tree", "Calkin-Wilf tree" });
    foreach (var tree in trees)
    {
        var restoredFraction = tree.First.FindFractionByPath(targetPath);
        Console.WriteLine($"Total bits for numerator and denominator as numbers == {GetFractionBitsCount(restoredFraction)}");

        //var continuedFraction = restoredFraction.ToContinuedFraction();
        //Console.Write($"Continued fraction: ");
        //foreach (var continuedFractionComponent in continuedFraction)
        //{
        //    Console.Write($"{continuedFractionComponent} ");
        //}
        //Console.WriteLine();

        Console.WriteLine($"Restored fraction by path with {tree.Second}: {restoredFraction}");
        var restoredPath = tree.First.FindPathByFraction(restoredFraction);
        Console.WriteLine("Restored path by restored fraction with {0} is {1}equal to initial path", tree.Second, restoredPath.SequenceEqual(targetPath)? string.Empty : "not ");
    }
}

void TestDavid(
    int? randomSource = null)
{
    randomSource ??= new Random().Next();
    var random = new Random(randomSource.Value);
    Console.WriteLine($"Seed: {randomSource.Value}");
    
    var cwTree = new CalkinWilfTree();
    var sbTree = new SternBrokotTree();
    
    var fraction = new BigInteger[] { new (128), new (127), new (127), new (126), new (125), new (123), new (122), new (122), new (120), new (119), new (118), new (117), new (117), new (116), new (115), new (110), new (105), new (101), new (98), new (97) }.ToFraction();
    Console.Write(fraction);
    Console.WriteLine($", total bits for numerator and denominator as numbers == {GetFractionBitsCount(fraction)}");

    var cwTreePath = cwTree.FindPathByFraction(fraction);
    var sbTreePath = sbTree.FindPathByFraction(fraction);
    Console.WriteLine($"Calkin-Wilf tree representation length = {cwTreePath.BitsCount}");
    Console.WriteLine($"Stern-Brokot tree representation length = {sbTreePath.BitsCount}");
}

void TestWhoIsShorter(
    int iterationsCount = 1000,
    int pathLengthMinBound = 1000,
    int pathLengthMaxBound = 2000,
    int? randomSource = null)
{
    randomSource ??= new Random().Next();
    var random = new Random(randomSource.Value);
    Console.WriteLine($"Seed: {randomSource.Value}");
    
    var cwTree = new CalkinWilfTree();
    var sbTree = new SternBrokotTree();
    
    for (var i = 1; i <= iterationsCount; i++)
    {
        var pathLength = random.Next(pathLengthMinBound, pathLengthMaxBound);
        var bits = Enumerable
            .Repeat(0, pathLength)
            .Select(_ => random.Next(2) == 0)
            .ToArray();
        var targetPath = new BitArray(bits);
        var sbTreeFraction = sbTree.FindFractionByPath(targetPath);
        var fractionBitsCount = GetFractionBitsCount(sbTreeFraction);
        var comparisonResult = fractionBitsCount.CompareTo(targetPath.BitsCount);
        Console.Write($"[{i:00000}][Stern-Brokot] path length = {targetPath.BitsCount:00000}, fraction length = {fractionBitsCount:00000} => ");
        Console.Write(comparisonResult switch
        {
            0 => "Equal",
            < 0 => "Fraction",
            _ => "Path"
        });
        Console.WriteLine(" is shorter.");
        
        var cwTreeFraction = cwTree.FindFractionByPath(targetPath);
        fractionBitsCount = GetFractionBitsCount(cwTreeFraction);
        comparisonResult = fractionBitsCount.CompareTo(targetPath.BitsCount);
        Console.Write($"[{i:00000}][Calkin-Wilf] path length = {targetPath.BitsCount:00000}, fraction length = {fractionBitsCount:00000} => ");
        Console.Write(comparisonResult switch
        {
            0 => "Equal",
            < 0 => "Fraction",
            _ => "Path"
        });
        Console.WriteLine(" is shorter.");
    }
}

void TestSternBrokotTreePaths()
{
    IFractionTree tree = new SternBrokotTree();
    var builder = new StringBuilder();

    for (var i = 1; i <= 11; i++)
    {
        for (var j = (int)BigInteger.Pow(2, i - 1) - 1; j >= 0; j--)
        {
            var str = Convert.ToString(j, 2);
            builder
                .Append(new string(Enumerable.Repeat('0', i - (str.Length > i ? i : str.Length)).ToArray()))
                .Append(str);
            var bitArray = new BitArray(builder.ToString().Select(x => int.Parse(x.ToString()) == 0));
            builder.Clear();
            var fraction = tree.FindFractionByPath(bitArray);
            var difference = fraction.Numerator - fraction.Denominator;

            Console.Write($"{fraction} ");
        }
    
        Console.WriteLine();
    }
}

void TestVertexIncidence()
{
    var hg = new HomogenousHypergraph(10, 4, new HyperEdge[] { new (0, 1, 2, 3), new (0, 1, 2, 4), new (0, 1, 2, 5), new (0, 1, 2, 6), new (0, 1, 2, 7), new (0, 1, 2, 8), new (0, 1, 2, 9), new (0, 1, 3, 4), new (0, 1, 3, 5), new (0, 1, 3, 6), new (0, 1, 3, 7), new (0, 1, 3, 8), new (0, 1, 3, 9), new (0, 1, 4, 5), new (0, 1, 4, 6), new (0, 1, 4, 7), new (0, 1, 4, 8), new (0, 1, 4, 9), new (0, 1, 5, 6), new (0, 1, 5, 7), new (0, 1, 5, 8), new (0, 1, 5, 9), new (0, 1, 6, 7), new (0, 1, 6, 8), new (0, 1, 6, 9), new (0, 1, 7, 8), new (0, 1, 7, 9), new (0, 1, 8, 9) });

    foreach (var simplex in hg.GetSimplicesIncidentToVertex(9))
    {
        Console.WriteLine($"{{{simplex}}}");
    }
}

void TestEncryptionByteArray()
{
    var algorithm = new HomogenousHypergraphEncryptor(new HomogenousHypergraph(4, 3, new (0, 1, 2), new (0, 1, 3)), 2);

    var block = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
    Console.Write("Initial state: ");
    for (var i = 0; i < block.Length; ++i)
    {
        Console.Write($"{block[i]} ");
    }
    Console.WriteLine();

    CryptoTransformationContext.PerformCipher(algorithm, 1, CipherMode.ElectronicCodebook, CipherTransformationMode.Encryption, null, block);
    Console.Write("Encryption result: ");
    for (var i = 0; i < block.Length; ++i)
    {
        Console.Write($"{block[i]} ");
    }
    Console.WriteLine();

    CryptoTransformationContext.PerformCipher(algorithm, 1, CipherMode.ElectronicCodebook, CipherTransformationMode.Decryption, null, block);
    Console.Write("Decryption result: ");
    for (var i = 0; i < block.Length; ++i)
    {
        Console.Write($"{block[i]} ");
    }
    Console.WriteLine();
}

async Task CreateRandomFileAsync(
    string filePath,
    CancellationToken cancellationToken = default)
{
    var randomSource = new Random();
    var bytes = new byte[1024];
    await using var writer = new FileStream(filePath, FileMode.Create, FileAccess.Write);
    
    for (var i = 0; i < 1024; ++i)
    {
        for (var j = 0; j < 1024; ++j)
        {
            randomSource.NextBytes(bytes);
            await writer.WriteAsync(bytes.AsMemory(0, bytes.Length), cancellationToken).ConfigureAwait(false);
        }
    }
}

async Task TestFileEncryptionAsync(
    string inputFilePath,
    string encryptedFilePath,
    string decryptedFilePath,
    HomogenousHypergraph key,
    int coresToUseCount,
    int smallBlockSize,
    CipherMode cipherMode,
    PaddingMode paddingMode,
    byte[] initializationVector,
    CancellationToken cancellationToken = default)
{
    var algorithm = new HomogenousHypergraphEncryptor(key, smallBlockSize);

    await CryptoTransformationContext.PerformCipherAsync(algorithm, coresToUseCount, cipherMode, paddingMode, CipherTransformationMode.Encryption, initializationVector, inputFilePath, encryptedFilePath, cancellationToken).ConfigureAwait(false);
    await CryptoTransformationContext.PerformCipherAsync(algorithm, coresToUseCount, cipherMode, paddingMode, CipherTransformationMode.Decryption, initializationVector, encryptedFilePath, decryptedFilePath, cancellationToken).ConfigureAwait(false);
}

async Task<string?> CheckFilesEqualityAsync(
    string initialFilePath,
    string decryptedFilePath,
    CancellationToken cancellationToken = default)
{
    await using var inputFileReader = new FileStream(initialFilePath, FileMode.Open, FileAccess.Read);
    await using var decryptedFileReader = new FileStream(decryptedFilePath, FileMode.Open, FileAccess.Read);

    if (inputFileReader.Length != decryptedFileReader.Length)
    {
        return $"Not eq by length: input len == {inputFileReader.Length}, decrypted len == {decryptedFileReader.Length}";
    }

    const int blockSize = 1024;

    var inputFileBytes = new byte[blockSize];
    var decryptedFileBytes = new byte[blockSize];
    var blockIndex = 0;
    var byteIndex = 0;
    
    while (byteIndex < inputFileReader.Length)
    {
        byteIndex += await inputFileReader.ReadAsync(inputFileBytes.AsMemory(0, inputFileBytes.Length), cancellationToken).ConfigureAwait(false);
        await decryptedFileReader.ReadAsync(decryptedFileBytes.AsMemory(0, decryptedFileBytes.Length), cancellationToken).ConfigureAwait(false);

        if (!inputFileBytes.SequenceEqual(decryptedFileBytes))
        {
            return $"Not eq by value, block # == {blockIndex}";
        }

        ++blockIndex;
    }

    return null;
}

async Task TestEncryptionAsync(
    string initialFilePath,
    CancellationToken cancellationToken = default)
{
    string GetShortFriendlyCipherTransformationModeName(
        CipherTransformationMode cipherTransformationMode)
    {
        return cipherTransformationMode switch
        {
            CipherTransformationMode.Encryption => "ENC",
            CipherTransformationMode.Decryption => "DEC",
            _ => throw new ArgumentOutOfRangeException(nameof(cipherTransformationMode))
        };
    }

    string GetFriendlyCipherModeName(
        CipherMode _cipherMode)
    {
        return _cipherMode switch
        {
            CipherMode.ElectronicCodebook => "ECB",
            CipherMode.CipherBlockChaining => "CBC",
            CipherMode.PropagatingCipherBlockChaining => "PCBC",
            CipherMode.CipherFeedback => "CFB",
            CipherMode.OutputFeedback => "OFB",
            CipherMode.CounterMode => "CTR",
            CipherMode.RandomDelta => "RD",
            _ => throw new ArgumentOutOfRangeException(nameof(_cipherMode))
        };
    }
        
    string GetFriendlyPaddingModeName(
        PaddingMode _paddingMode)
    {
        return _paddingMode switch
        {
            PaddingMode.Zeros => "Zeros",
            PaddingMode.ISO10126 => "ISO10126",
            PaddingMode.ANSIX923 => "ANSIX923",
            PaddingMode.PKCS7 => "PKCS7",
            _ => throw new ArgumentOutOfRangeException(nameof(_paddingMode))
        };
    }
    
    string ConstructFilePath(
        string initialFilePath,
        CipherTransformationMode cipherTransformationMode, 
        int coresToUseCount,
        int smallBlockSize,
        int keyIndex,
        int initializationVectorIndex,
        CipherMode cipherMode,
        PaddingMode paddingMode)
    {

        var initialFilePathInfo = new FileInfo(initialFilePath);
        var fileDirectory = initialFilePathInfo.Directory!;
        var fileName = Path.GetFileNameWithoutExtension(initialFilePath);
        var fileExtension = initialFilePathInfo.Extension;
        
        return Path.Combine(fileDirectory.FullName, $"{fileName}_crs{coresToUseCount}_sbs{smallBlockSize}_key{keyIndex}_iv{initializationVectorIndex}_cmd{GetFriendlyCipherModeName(cipherMode)}_pmd{GetFriendlyPaddingModeName(paddingMode)}_{GetShortFriendlyCipherTransformationModeName(cipherTransformationMode)}{fileExtension}");
    }
    
    string ConstructLogPrefix(
        int coresToUseCount,
        int smallBlockSize,
        int keyIndex,
        int initializationVectorIndex,
        CipherMode cipherMode,
        PaddingMode paddingMode)
    {
        return $"Cores to use count = {coresToUseCount}, small block size = {smallBlockSize}, key id = {keyIndex}, IV index = {initializationVectorIndex}, cipher mode = {GetFriendlyCipherModeName(cipherMode)}, padding mode = {GetFriendlyPaddingModeName(paddingMode)}";
    }
    
    var coresCountEnumerable = Enumerable.Range(1, 8);
    var smallBlocksSizes = Enumerable.Range(1, 10);
    var keys = new HomogenousHypergraph[]
    {
        new (6, 3, new(0, 1, 2), new (0, 1, 3), new (0, 1, 4), new (0, 1, 5), new (0, 2, 5), new (0, 3, 4), new (0, 4, 5)),
        new (6, 3, new (0, 4, 5), new (1, 2, 3), new (3, 4, 5)),
        new (6, 3, new (0, 1, 2), new (1, 2, 3), new (2, 3, 4), new (0, 4, 5))
    };
    var cipherModes = Enum.GetValues<CipherMode>();
    var paddingModes = Enum.GetValues<PaddingMode>();
    var initializationVectors = new[]
    {
        new byte[18],
        new byte[18],
        new byte[36],
        new byte[36],
        new byte[54],
        new byte[54]
    };
    var randomSource = new Random(123456);
    foreach (var initializationVector in initializationVectors)
    {
        randomSource.NextBytes(initializationVector);
    }
    
    var cipherTasks = coresCountEnumerable
        .CartesianProduct(smallBlocksSizes)
        .CartesianProduct(Enumerable.Range(0, keys.Length))
        .CartesianProduct(cipherModes)
        .CartesianProduct(paddingModes)
        .CartesianProduct(Enumerable.Range(0, initializationVectors.Length))
        .Select(parameters =>
        {
            var coresToUseCount = parameters.Item1.Item1.Item1.Item1.Item1;
            var smallBlockSize = parameters.Item1.Item1.Item1.Item1.Item2;
            var keyIndex = parameters.Item1.Item1.Item1.Item2;
            var cipherMode = parameters.Item1.Item1.Item2;
            var paddingMode = parameters.Item1.Item2;
            var initializationVectorIndex = parameters.Item2;
        
            var encryptedFilePath = ConstructFilePath(initialFilePath, CipherTransformationMode.Encryption, coresToUseCount, smallBlockSize, keyIndex, initializationVectorIndex, cipherMode, paddingMode);
            var decryptedFilePath = ConstructFilePath(initialFilePath, CipherTransformationMode.Decryption, coresToUseCount, smallBlockSize, keyIndex, initializationVectorIndex, cipherMode, paddingMode);

            async Task<string> CipherAsync(
                CancellationToken cancellationTokenInner = default)
            {
                try
                {
                    await TestFileEncryptionAsync(initialFilePath, encryptedFilePath, decryptedFilePath, keys[keyIndex], coresToUseCount, smallBlockSize, cipherMode, paddingMode, initializationVectors[initializationVectorIndex], cancellationTokenInner).ConfigureAwait(false);
                    var filesComparisonResultLog = await CheckFilesEqualityAsync(initialFilePath, decryptedFilePath, cancellationTokenInner).ConfigureAwait(false);
                    
                    return $"{ConstructLogPrefix(coresToUseCount, smallBlockSize, keyIndex, initializationVectorIndex, cipherMode, paddingMode)}: {(filesComparisonResultLog != null ? $"unexpected behavior: {filesComparisonResultLog}" : "OK")}";
                }
                catch (Exception ex)
                {
                    File.Delete(encryptedFilePath);
                    File.Delete(decryptedFilePath);
                    
                    return $"{ConstructLogPrefix(coresToUseCount, smallBlockSize, keyIndex, initializationVectorIndex, cipherMode, paddingMode)}: exception: {ex.Message}";
                }
            }

            return CipherAsync(cancellationToken);
        })
        .ToArray();

    await Task.WhenAll(cipherTasks);

    foreach (var cipherTask in cipherTasks)
    {
        Console.WriteLine(cipherTask.Result);
    }
}

void TestKeyDecompression()
{
    var initial = new VerticesDegreesVector(14, 13, 13, 13, 13, 12, 12, 12, 12, 11, 11, 10, 9, 9, 8);
    var diffsTransformed = VerticesDegreesVectorCompressor.ToDiffArray(initial);
    var transformed = VerticesDegreesVectorCompressor.Compress(initial);
    var diffsTransformedBack = VerticesDegreesVectorCompressor.FromDiffArray(diffsTransformed);
    var transformedBack = VerticesDegreesVectorCompressor.Decompress(transformed);
    
    var sb = new SternBrokotTree();
    var cf = diffsTransformed.Select(x => new BigInteger(x)).ToFraction();
    var path = sb.FindPathByFraction(cf);
    var cf2 = sb.FindFractionByPath(path);
    var diffsFromCf = cf2.ToContinuedFraction().Select(x => (int)x).ToArray();
    
    foreach (var item in initial)
    {
        Console.Write($"{item} ");
    }
    Console.WriteLine();
    foreach (var item in diffsTransformed)
    {
        Console.Write($"{item} ");
    }
    Console.WriteLine();
    foreach (var item in transformed)
    {
        Console.Write($"{(item ? 1 : 0)}");
    }
    Console.WriteLine();
    foreach (var item in diffsTransformedBack)
    {
        Console.Write($"{item} ");
    }
    Console.WriteLine();
    foreach (var item in transformedBack)
    {
        Console.Write($"{item} ");
    }
}

void TestVerticesDegreesVectorConstruction(
    int simplicesDimension,
    int verticesCount)
{
    BigInteger count = 0;
    foreach (var generatedVerticesDegreesVector in VerticesDegreesVectorConstructor.ConstructAllExtremalVerticesDegreesVectors(verticesCount, simplicesDimension))
    {
        ++count;
        foreach (var item in generatedVerticesDegreesVector)
        {
            Console.Write($"{item} ");
        }
        Console.WriteLine();
    }
    Console.Write($"Count: {count}");
}

// TestBitsCountInBitArray();
// TestHomogenousHypergraphConstruction();
// TestHomogenousHypergraphIteration();
// TestHomogenousHypergraphEtc();
// TestContinuedFraction(new Fraction(44435, 10587));
// TestFractionTrees();
// TestDavid();
// TestWhoIsShorter();
// TestSternBrokotTreePaths();
// TestVertexIncidence();
// TestEncryptionByteArray();
// await TestEncryptionFile(@"C:\Users\scarl\University\Irbis.PhDThesis\kek.txt", @"C:\Users\scarl\University\Irbis.PhDThesis\enc.txt", @"C:\Users\scarl\University\Irbis.PhDThesis\dec.txt");
// await TestEncryptionAsync(@"C:\Users\scarl\University\Irbis.PhDThesis\Files\Initial.jpg");
TestKeyDecompression();
// TestVerticesDegreesVectorConstruction(2, 7);

static class VerticesDegreesVectorCompressor
{
    
    private static readonly SternBrokotTree _fractionTree = new ();

    public static BitArray Compress(
        VerticesDegreesVector vector)
    {
        var diffArray = ToDiffArray(vector);
        var firstValue = diffArray[0];
        var firstValueBitsCount = 0;
        while (firstValue != 0)
        {
            ++firstValueBitsCount;
            firstValue >>= 1;
        }
        
        var result = new BitArray(sizeof(byte) * 8 + firstValueBitsCount + diffArray.Skip(1).Sum() - 1);

        for (var i = 0; i < sizeof(byte) * 8; ++i)
        {
            result[i] = ((firstValueBitsCount >> i) & 1) == 1;
        }
        
        for (var i = 0; i < firstValueBitsCount; ++i)
        {
            result[i + sizeof(byte) * 8] = ((diffArray[0] >> i) & 1) == 1;
        }

        var convertedPath = _fractionTree.FindPathByFraction(diffArray.Skip(1).Select(x => new BigInteger(x)).ToFraction());
        foreach (var convertedPathBit in convertedPath)
        {
            result[sizeof(byte) * 8 + firstValueBitsCount++] = convertedPathBit;
        }

        return result;
    }

    public static VerticesDegreesVector Decompress(
        BitArray transformedVerticesDegreesVector)
    {
        byte firstValueBitsCount = 0;
        for (var i = 0; i < sizeof(byte) * 8; ++i)
        {
            if (!transformedVerticesDegreesVector[i])
            {
                continue;
            }

            firstValueBitsCount |= (byte)(1 << i);
        }

        int firstValue = 0;
        for (var i = 0; i < firstValueBitsCount; ++i)
        {
            if (!transformedVerticesDegreesVector[sizeof(byte) * 8 + i])
            {
                continue;
            }

            firstValue |= 1 << i;
        }

        var diffArray = _fractionTree.FindFractionByPath(
                new BitArray(transformedVerticesDegreesVector.Skip(sizeof(byte) * 8 + firstValueBitsCount)))
            .ToContinuedFraction()
            .Select(x => (int)x)
            .Prepend(firstValue)
            .ToArray();

        return FromDiffArray(diffArray);
    }

    public static int[] ToDiffArray(
        VerticesDegreesVector vector)
    {
        var degreesValues = vector.Reverse().ToArray();
        var result = new int[degreesValues.Length];

        var previousDegree = result[0] = degreesValues.First();
        degreesValues.Skip(1).ForEach((currentDegree, index) =>
        {
            result[index + 1] = currentDegree - previousDegree + 1;
            previousDegree = currentDegree;
        });
        ++result[^1];
        
        return result;
    }

    public static VerticesDegreesVector FromDiffArray(
        int[] diffArray)
    {
        var modifiedDiffsArray = (int[])diffArray.Clone();
        --modifiedDiffsArray[^1];
        
        for (var i = 1; i < modifiedDiffsArray.Length; ++i)
        {
            modifiedDiffsArray[i] += modifiedDiffsArray[i - 1];
            --modifiedDiffsArray[i];
        }

        return new VerticesDegreesVector(modifiedDiffsArray.Reverse().ToArray());
    }
    
}

static class VerticesDegreesVectorConstructor
{
    
    public static IEnumerable<VerticesDegreesVector> ConstructAllVerticesDegreesVectors(
        int verticesCount,
        int simplicesDimension)
    {
        IEnumerable<VerticesDegreesVector> Generate(
            int[] resultVerticesDegreesVectorCoefficients,
            BigInteger setUpVerticesDegreesSum,
            int vertexDegreeIndexToSetUp,
            BigInteger maxVertexDegree)
        {
            if (setUpVerticesDegreesSum < BigInteger.Zero)
            {
                yield break;
            }
        
            if (vertexDegreeIndexToSetUp == resultVerticesDegreesVectorCoefficients.Length)
            {
                if (setUpVerticesDegreesSum == 0)
                {
                    yield return new VerticesDegreesVector(resultVerticesDegreesVectorCoefficients);
                }

                yield break;
            }

            for (var i = 1; i <= (vertexDegreeIndexToSetUp == 0
                     ? maxVertexDegree
                     : resultVerticesDegreesVectorCoefficients[vertexDegreeIndexToSetUp - 1]); ++i)
            {
                resultVerticesDegreesVectorCoefficients[vertexDegreeIndexToSetUp] = i;
                foreach (var generatedVerticesDegreesVector in Generate(resultVerticesDegreesVectorCoefficients, setUpVerticesDegreesSum - i, vertexDegreeIndexToSetUp + 1, maxVertexDegree))
                {
                    yield return generatedVerticesDegreesVector;
                }
            }
        }
        
        var maxVertexDegree = BigIntegerExtensions.CombinationsCount(verticesCount - 1, simplicesDimension - 1);
        var maxSum = verticesCount * maxVertexDegree;

        // TODO: Guardant

        for (var sum = 0; sum <= maxSum; sum += simplicesDimension)
        {
            foreach (var generatedVerticesDegreesVector in Generate(new int[verticesCount], sum, 0, maxVertexDegree))
            {
                yield return generatedVerticesDegreesVector;
            }
        }
    }

    public static IEnumerable<VerticesDegreesVector> ConstructAllConnectedVerticesDegreesVectors(
        int verticesCount,
        int simplicesDimension)
    {
        // TODO: implement me plz
        return ConstructAllVerticesDegreesVectors(verticesCount, simplicesDimension)
            .Select(x => x);
    }

    public static IEnumerable<VerticesDegreesVector> ConstructAllTheOnlyWayRestorableVerticesDegreesVectors(
        int verticesCount,
        int simplicesDimension)
    {
        // TODO: implement me plz
        return ConstructAllVerticesDegreesVectors(verticesCount, simplicesDimension)
            .Select(x => x);
    }

    public static IEnumerable<VerticesDegreesVector> ConstructAllExtremalVerticesDegreesVectors(
        int verticesCount,
        int simplicesDimension)
    {
        // TODO: implement me plz
        return ConstructAllVerticesDegreesVectors(verticesCount, simplicesDimension)
            .Select(x => x);
    }
    
}