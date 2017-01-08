using System;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace Gwen.MonoGame.XmlContentPipeline
{
	public class GwenXmlWriter : ContentTypeWriter<byte[]>
	{
		protected override void Write(ContentWriter output, byte[] value)
		{
			output.Write(value);
		}

		public override string GetRuntimeType(TargetPlatform targetPlatform)
		{
			return typeof(string).AssemblyQualifiedName;
		}

		public override string GetRuntimeReader(TargetPlatform targetPlatform)
		{
			return "Gwen.MonoGame.GwenXmlReader";
		}
	}
}
