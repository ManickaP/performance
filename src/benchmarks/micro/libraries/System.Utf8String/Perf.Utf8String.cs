﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using MicroBenchmarks;
using System.IO;

namespace System.Text
{
    [BenchmarkCategory(Categories.Libraries, Categories.Runtime, Categories.NoMono)]
    public class Perf_Utf8String : Perf_TextBase
    {
        private Utf8String _utf8;
        private Memory<char> _destination;

        [GlobalSetup]
        public void Setup()
        {
            string unicode = File.ReadAllText(Path.Combine(TextFilesRootPath, $"{Input}.txt"));
            _utf8 = new Utf8String(unicode);
            _destination = new char[unicode.Length];
        }

        [Benchmark]
        public int ToChars() => new Utf8Span(_utf8).ToChars(_destination.Span);

        [Benchmark]
        public bool IsAscii() => new Utf8Span(_utf8).IsAscii();

        [Benchmark]
        public bool IsNormalized() => new Utf8Span(_utf8).IsNormalized();

        [Benchmark]
        public char[] ToCharArray() => new Utf8Span(_utf8).ToCharArray();
    }
}
