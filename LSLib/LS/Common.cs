using System;
using System.IO;
using System.Text.RegularExpressions;

namespace LSLib.LS
{
	public static class Common
	{
		public const int MajorVersion = 1;

		public const int MinorVersion = 18;

		public const int PatchVersion = 7;

		// Version of LSTools profile data in generated DAE files
        public const int ColladaMetadataVersion = 3;

        /// <summary>
        /// Returns the version number of the LSLib library
        /// </summary>
        public static string LibraryVersion()
		{
			return String.Format("{0}.{1}.{2}", MajorVersion, MinorVersion, PatchVersion);
		}

		/// <summary>
		/// Compares the string against a given pattern.
		/// </summary>
		/// <param name="str">The string</param>
		/// <param name="pattern">The pattern to match, where "*" means any sequence of characters, and "?" means any single character</param>
		/// <returns><c>true</c> if the string matches the given pattern; otherwise <c>false</c>.</returns>
		public static bool Like(this string str, string pattern)
		{
			return new Regex("^" + Regex.Escape(pattern).Replace(@"\*", ".*").Replace(@"\?", ".") + "$", RegexOptions.Singleline).IsMatch(str);
		}

		/// <summary>
		/// Compares the string against a given pattern.
		/// </summary>
		/// <param name="str">The string</param>
		/// <param name="pattern">The pattern to match as a RegEx object</param>
		/// <returns><c>true</c> if the string matches the given pattern; otherwise <c>false</c>.</returns>
		public static bool Like(this string str, Regex pattern)
		{
			return pattern.IsMatch(str);
		}

		/// <summary>
		/// Creates a relative path from one file or folder to another.
		/// Source: https://stackoverflow.com/a/340454/2290477
		/// </summary>
		/// <param name="fromPath">Contains the directory that defines the start of the relative path.</param>
		/// <param name="toPath">Contains the path that defines the endpoint of the relative path.</param>
		/// <returns>The relative path from the start directory to the end path or <c>toPath</c> if the paths are not related.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="UriFormatException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		public static string GetRelativePath(string fromPath, string toPath)
		{
			if (String.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
			if (String.IsNullOrEmpty(toPath)) throw new ArgumentNullException("toPath");

			var fromUri = new Uri(fromPath);
			var toUri = new Uri(toPath);

			if (fromUri.Scheme != toUri.Scheme) { return toPath; } // path can't be made relative.

			var relativeUri = fromUri.MakeRelativeUri(toUri);
			var relativePath = Uri.UnescapeDataString(relativeUri.ToString());

			if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
			{
				relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
			}

			return relativePath;
		}
	}
}
