// Modified from https://github.com/xunit/xunit/blob/3fc07b2bd9eb624f7baaa65d1be63ae574b737a1/src/xunit.core/Record.cs
/*
 * Copyright (c) .NET Foundation and Contributors
 * All Rights Reserved
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Threading.Tasks;

static class Record
{
    public static Exception Exception(Action testCode)
    {
        if (testCode == null)
            throw new ArgumentNullException(nameof(testCode));

        try
        {
            testCode();
            return null;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public static async Task<Exception> ExceptionAsync(Func<Task> testCode)
    {
        if (testCode == null)
            throw new ArgumentNullException(nameof(testCode));

        try
        {
            await testCode();
            return null;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}