using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace Gwen.MonoGame.XmlContentPipeline
{
	[ContentProcessor(DisplayName = "Gwen Xml Processor")]
	public class GwenXmlProcessor : ContentProcessor<string, byte[]>
	{
		public string Key { get; set; } = "";

		public override byte[] Process(string input, ContentProcessorContext context)
		{
			try
			{
				byte[] bytes = TextToBinary(input);

				if (!String.IsNullOrWhiteSpace(Key))
				{
					byte[] keyBytes = Enumerable.Range(0, Key.Length / 2).Select(x => Convert.ToByte(Key.Substring(x * 2, 2), 16)).ToArray();

					for (int i = 0; i < bytes.Length; i++)
					{
						bytes[i] = (byte)(bytes[i] ^ keyBytes[i % keyBytes.Length]);
					}
				}

				return bytes;
			}
			catch (Exception ex)
			{
				context.Logger.LogMessage("Error {0}", ex);
				throw;
			}
		}

		private byte[] TextToBinary(string text)
		{
			var inputBytes = Encoding.UTF8.GetBytes(text);
			byte[] outputBytes = null;
			using (var outputStream = new MemoryStream())
			{
				using (var stream = new GZipStream(outputStream, CompressionMode.Compress))
				{
					stream.Write(inputBytes, 0, inputBytes.Length);
				}

				outputBytes = outputStream.ToArray();
			}

			return outputBytes;
		}
	}
}
