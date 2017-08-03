using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net.ObjectRenderer;

namespace toofz.NecroDancer.Leaderboards
{
    /// <summary>
    /// Custom log4net renderer for <see cref="Exception"/>.
    /// </summary>
    public sealed class ExceptionRenderer : IObjectRenderer
    {
        /// <summary>
        /// Renders an object of type <see cref="Exception"/> similar to how the Visual Studio Exception Assistant 
        /// renders it.
        /// </summary>
        /// <param name="rendererMap">Not used.</param>
        /// <param name="obj">The exception.</param>
        /// <param name="writer">The writer.</param>
        public void RenderObject(RendererMap rendererMap, object obj, TextWriter writer)
        {
            var ex = (Exception)obj;
            var type = ex.GetType();

            var indentedWriter = writer as IndentedTextWriter;
            if (indentedWriter == null)
            {
                indentedWriter = new IndentedTextWriter(writer, "  ");

                indentedWriter.Write($"{type} was unhandled");
                indentedWriter.Indent++;
            }

            var properties = type.GetProperties().OrderBy(x => x.Name);
            foreach (var property in properties)
            {
                var name = property.Name;

                switch (name)
                {
                    // Ignored properties
                    case "Data":
                    case "TargetSite":

                    // Special case properties
                    case "StackTrace":
                    case "InnerException":
                        break;

                    default:
                        var value = property.GetValue(ex)?.ToString();
                        if (value != null)
                        {
                            indentedWriter.WriteLineStart($"{name}={value}");
                        }
                        break;
                }
            }

            WriteStackTrace(indentedWriter, new StackTrace(ex));

            var innerException = ex.InnerException;
            if (innerException != null)
            {
                type = innerException.GetType();
                indentedWriter.WriteLineStart($"InnerException: {type}");
                indentedWriter.Indent++;
                RenderObject(rendererMap, innerException, indentedWriter);
            }
        }

        void WriteStackTrace(IndentedTextWriter indentedWriter, StackTrace stackTrace)
        {
            if (stackTrace.FrameCount == 0)
                return;

            indentedWriter.WriteLineStart("StackTrace:");
            indentedWriter.Indent++;

            foreach (StackFrame frame in stackTrace.GetFrames())
            {
                MethodBase method = frame.GetMethod();
                Type type = method.DeclaringType;

                if (type.Namespace != "System.Runtime.CompilerServices")
                {
                    var methodSig = GetMethodSignature(method);

                    var value = type.Namespace != null ?
                        string.Join(".", type.Namespace, type.Name, methodSig) :
                        string.Join(".", type.Name, methodSig);
                    indentedWriter.WriteLineStart(value);
                }
            }

            indentedWriter.Indent--;
        }

        static string GetMethodSignature(MethodBase method)
        {
            var fullSig = method.ToString();
            var i = fullSig.IndexOf(' ');

            return fullSig.Substring(i + 1);
        }
    }
}
