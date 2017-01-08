using System;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace Gwen.MonoGame.XmlContentPipeline
{
	[ContentImporter(".xml", DefaultProcessor = "GwenXmlProcessor", DisplayName = "Gwen Xml Importer")]
	public class GwenXmlImporter : ContentImporter<string>
	{
		public override string Import(string filename, ContentImporterContext context)
		{
			context.Logger.LogMessage("Importing Gwen Xml file: {0}", filename);

			using (var streamReader = new StreamReader(filename))
			{
				return streamReader.ReadToEnd();
			}
		}
	}
}
