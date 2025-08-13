using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

namespace SolarForge.Scenarios
{

	public class NodeGraphicsManager : IDisposable
	{

		public NodeGraphicsManager(string basePath = null)
		{
			this.imageCache = new Dictionary<string, Image>();
			this.sizedImageCache = new Dictionary<string, Dictionary<int, Image>>();
			this.graphicsBasePath = (basePath ?? Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "", "NodeGraphics"));
		}


		public Image GetNodeGraphic(string fillingName, int size)
		{
			if (string.IsNullOrEmpty(fillingName))
			{
				return null;
			}
			string text = this.FindImageForFillingName(fillingName);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			return this.GetScaledImage(text, size);
		}


		public bool HasCustomGraphic(string fillingName)
		{
			return !string.IsNullOrEmpty(fillingName) && !string.IsNullOrEmpty(this.FindImageForFillingName(fillingName));
		}


		public void ClearCache()
		{
			foreach (Image image in this.imageCache.Values)
			{
				if (image != null)
				{
					image.Dispose();
				}
			}
			this.imageCache.Clear();
			foreach (Dictionary<int, Image> dictionary in this.sizedImageCache.Values)
			{
				foreach (Image image2 in dictionary.Values)
				{
					if (image2 != null)
					{
						image2.Dispose();
					}
				}
				dictionary.Clear();
			}
			this.sizedImageCache.Clear();
		}


		public string GetCacheInfo()
		{
			int count = this.imageCache.Count;
			int num = 0;
			foreach (Dictionary<int, Image> dictionary in this.sizedImageCache.Values)
			{
				num += dictionary.Count;
			}
			return string.Format("NodeGraphicsManager Cache: {0} original images, {1} scaled images", count, num);
		}


		public string GetDebugInfo(string fillingName)
		{
			if (string.IsNullOrEmpty(fillingName))
			{
				return "FillingName is null or empty";
			}
			List<string> values = this.GenerateCandidateNames(fillingName);
			return string.Concat(new string[]
			{
				"FillingName: '",
				fillingName,
				"', Candidates: [",
				string.Join(", ", values),
				"]"
			});
		}


		private string FindImageForFillingName(string fillingName)
		{
			if (!Directory.Exists(this.graphicsBasePath))
			{
				return null;
			}
			foreach (string str in this.GenerateCandidateNames(fillingName))
			{
				foreach (string str2 in NodeGraphicsManager.SupportedExtensions)
				{
					string text = Path.Combine(this.graphicsBasePath, str + str2);
					if (File.Exists(text))
					{
						return text;
					}
				}
			}
			return null;
		}


		private List<string> GenerateCandidateNames(string fillingName)
		{
			List<string> list = new List<string>();
			if (!string.IsNullOrEmpty(fillingName))
			{
				list.Add(fillingName.ToLowerInvariant());
				list.Add(fillingName.ToLowerInvariant().Replace(" ", "_"));
				list.Add(fillingName.ToLowerInvariant().Replace(" ", "-"));
				list.Add(fillingName.ToLowerInvariant().Replace(" ", ""));
			}
			list.Add("default");
			list.Add("unknown");
			return list;
		}


		private Image GetScaledImage(string imagePath, int size)
		{
			Dictionary<int, Image> dictionary;
			Image result;
			if (this.sizedImageCache.TryGetValue(imagePath, out dictionary) && dictionary.TryGetValue(size, out result))
			{
				return result;
			}
			Image image;
			if (!this.imageCache.TryGetValue(imagePath, out image))
			{
				try
				{
					using (FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
					{
						image = new Bitmap(fileStream);
					}
					if (image.PixelFormat != PixelFormat.Format32bppArgb && image.PixelFormat != PixelFormat.Format32bppPArgb)
					{
						Bitmap bitmap = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
						using (Graphics graphics = Graphics.FromImage(bitmap))
						{
							graphics.Clear(Color.Transparent);
							graphics.DrawImage(image, 0, 0);
						}
						image.Dispose();
						image = bitmap;
					}
					this.imageCache[imagePath] = image;
				}
				catch (Exception)
				{
					return null;
				}
			}
			Image image2 = this.CreateScaledImage(image, size);
			if (!this.sizedImageCache.ContainsKey(imagePath))
			{
				this.sizedImageCache[imagePath] = new Dictionary<int, Image>();
			}
			this.sizedImageCache[imagePath][size] = image2;
			return image2;
		}


		private Image CreateScaledImage(Image original, int size)
		{
			Bitmap bitmap = new Bitmap(size, size, PixelFormat.Format32bppArgb);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.Clear(Color.FromArgb(0, 0, 0, 0));
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.AntiAlias;
				graphics.CompositingMode = CompositingMode.SourceOver;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.DrawImage(original, 0, 0, size, size);
			}
			if (original.PixelFormat == PixelFormat.Format24bppRgb || original.PixelFormat == PixelFormat.Format8bppIndexed)
			{
				bitmap.MakeTransparent(Color.White);
			}
			else
			{
				bitmap.MakeTransparent();
			}
			return bitmap;
		}


		public void Dispose()
		{
			if (!this.disposed)
			{
				this.ClearCache();
				this.disposed = true;
			}
		}


		private readonly Dictionary<string, Image> imageCache;


		private readonly Dictionary<string, Dictionary<int, Image>> sizedImageCache;


		private readonly string graphicsBasePath;


		private bool disposed;


		private static readonly string[] SupportedExtensions = new string[]
		{
			".png",
			".dds",
			".bmp",
			".jpg",
			".jpeg"
		};
	}
}
