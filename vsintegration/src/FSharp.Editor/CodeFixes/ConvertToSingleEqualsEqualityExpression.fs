﻿// Copyright (c) Microsoft Corporation.  All Rights Reserved.  See License.txt in the project root for license information.

namespace Microsoft.VisualStudio.FSharp.Editor

open System.Composition
open System.Collections.Immutable

open Microsoft.CodeAnalysis.Text
open Microsoft.CodeAnalysis.CodeFixes

open CancellableTasks

[<ExportCodeFixProvider(FSharpConstants.FSharpLanguageName, Name = CodeFix.ConvertToSingleEqualsEqualityExpression); Shared>]
type internal ConvertToSingleEqualsEqualityExpressionCodeFixProvider() =
    inherit CodeFixProvider()

    static let title = SR.ConvertToSingleEqualsEqualityExpression()

    override _.FixableDiagnosticIds = ImmutableArray.Create "FS0043"

    override this.RegisterCodeFixesAsync context = context.RegisterFsharpFix this

    interface IFSharpCodeFixProvider with
        member _.GetCodeFixIfAppliesAsync context =
            cancellableTask {
                let! text = context.GetSquigglyTextAsync()

                if text <> "==" then
                    return None
                else
                    return
                        Some
                            {
                                Name = CodeFix.ConvertToSingleEqualsEqualityExpression
                                Message = title
                                Changes = [ TextChange(context.Span, "=") ]
                            }
            }
