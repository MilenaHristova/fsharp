﻿// Copyright (c) Microsoft Corporation.  All Rights Reserved.  See License.txt in the project root for license information.

module FSharp.Editor.Tests.CodeFixes.AddMissingEqualsToTypeDefinitionTests

open Microsoft.VisualStudio.FSharp.Editor
open Xunit

open CodeFixTestFramework

let private codeFix = AddMissingEqualsToTypeDefinitionCodeFixProvider()
let private diagnostic = 0010 // Unexpected symbol...

[<Fact>]
let ``Fixes FS0010 for missing equals in type def - simple types`` () =
    let code =
        """
type Song { Artist : string; Title : int }
"""

    let expected =
        Some
            {
                Message = "Add missing '=' to type definition"
                FixedCode =
                    """
type Song = { Artist : string; Title : int }
"""
            }

    let actual = codeFix |> tryFix code diagnostic

    Assert.Equal(expected, actual)

[<Fact>]
let ``Fixes FS0010 for missing equals in type def - records`` () =
    let code =
        """
type Name Name of string
"""

    let expected =
        Some
            {
                Message = "Add missing '=' to type definition"
                FixedCode =
                    """
type Name = Name of string
"""
            }

    let actual = codeFix |> tryFix code diagnostic

    Assert.Equal(expected, actual)

[<Theory>]
[<InlineData "type X = open">]
[<InlineData "=">]
let ``Doesn't fix FS0010 for random unexpected symbols`` code =
    let expected = None

    let actual = codeFix |> tryFix code diagnostic

    Assert.Equal(expected, actual)
