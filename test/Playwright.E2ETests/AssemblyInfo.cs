using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NUnit.Framework;

// NUnit parallelization
[assembly: Parallelizable(ParallelScope.All)]
[assembly: LevelOfParallelism(1)]

