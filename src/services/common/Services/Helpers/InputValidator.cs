// <copyright file="InputValidator.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Text.RegularExpressions;
using Mmm.Iot.Common.Services.Exceptions;

namespace Mmm.Iot.Common.Services.Helpers
{
    public class InputValidator
    {
        private const string InvalidCharacterRegex = @"[^A-Za-z0-9:;.!,_\-*@ ]";

        // Check illegal characters in input
        public static void Validate(string input)
        {
            if (Regex.IsMatch(input.Trim(), InvalidCharacterRegex))
            {
                throw new InvalidInputException($"Input '{input}' contains invalid characters.");
            }
        }
    }
}