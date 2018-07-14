using System;

namespace StringCalculators
{
    public interface IStringCalculator
    {
        string Add(string input);
        string Add(in ReadOnlySpan<char> input);
    }
}