using System;
using System.Security.Permissions;
using System.Security.Policy;

namespace Loader.Shared
{
	internal class PublicKeys
	{
		public readonly static StrongName DotNetZip11010;

		public readonly static StrongName Ensage0001;

		public readonly static StrongName EnsageSharpSandbox1000;

		public readonly static StrongName JetBrainsAnnotations10140;

		public readonly static StrongName JetBrainsAnnotations10150;

		public readonly static StrongName LeagueSharp10025;

		public readonly static StrongName LeagueSharpSandbox1000;

		public readonly static StrongName log4net12150;

		public readonly static StrongName MonoCecil0960;

		public readonly static StrongName MonoSecurity4000;

		public readonly static StrongName NewtonsoftJson8000;

		public readonly static StrongName PlaySharpService1000;

		public readonly static StrongName PlaySharpToolkit1000;

		public readonly static StrongName SharpDXDirect3D92630;

		public readonly static StrongName SharpDX2630;

		public readonly static StrongName NewtonsoftJson9000;

		public readonly static StrongName MicrosoftVisualStudioThreading14000;

		public readonly static StrongName MicrosoftVisualStudioValidation14000;

		public readonly static StrongName SystemCollectionsImmutable1200;

		public readonly static StrongName PlaySharpAppDomainPluginLeagueSharp1000;

		public readonly static StrongName[] Keys;

		static PublicKeys()
		{
			PublicKeys.DotNetZip11010 = new StrongName(new StrongNamePublicKeyBlob(new byte[] { 0, 36, 0, 0, 4, 128, 0, 0, 148, 0, 0, 0, 6, 2, 0, 0, 0, 36, 0, 0, 82, 83, 65, 49, 0, 4, 0, 0, 1, 0, 1, 0, 53, 40, 211, 127, 225, 213, 39, 97, 179, 117, 9, 160, 139, 194, 80, 205, 207, 234, 70, 134, 136, 185, 242, 33, 238, 74, 114, 238, 178, 207, 250, 245, 82, 144, 90, 149, 137, 76, 145, 83, 76, 223, 61, 81, 97, 79, 6, 141, 237, 34, 98, 173, 54, 82, 143, 157, 110, 86, 134, 61, 249, 251, 185, 146, 200, 47, 193, 61, 216, 222, 241, 18, 178, 158, 110, 232, 164, 142, 179, 59, 164, 240, 132, 238, 17, 32, 13, 109, 28, 15, 189, 53, 124, 218, 165, 193, 77, 45, 51, 24, 148, 136, 42, 58, 232, 219, 206, 195, 48, 54, 99, 69, 174, 19, 129, 39, 176, 84, 211, 163, 242, 186, 107, 169, 183, 144, 173, 184 }), "DotNetZip", new Version(1, 10, 1, 0));
			PublicKeys.Ensage0001 = new StrongName(new StrongNamePublicKeyBlob(new byte[] { 0, 36, 0, 0, 4, 128, 0, 0, 148, 0, 0, 0, 6, 2, 0, 0, 0, 36, 0, 0, 82, 83, 65, 49, 0, 4, 0, 0, 1, 0, 1, 0, 45, 195, 43, 160, 36, 244, 232, 29, 165, 81, 57, 166, 53, 127, 100, 30, 154, 164, 238, 1, 99, 113, 26, 142, 120, 161, 202, 56, 38, 58, 119, 105, 123, 13, 163, 182, 51, 46, 211, 188, 26, 65, 216, 172, 15, 247, 203, 28, 29, 84, 110, 232, 133, 25, 235, 232, 179, 247, 153, 201, 249, 121, 129, 106, 236, 101, 71, 120, 76, 228, 29, 16, 25, 11, 111, 71, 240, 150, 107, 135, 3, 223, 177, 17, 149, 197, 94, 240, 101, 76, 92, 216, 90, 97, 166, 180, 81, 73, 49, 64, 15, 225, 181, 1, 245, 226, 101, 79, 146, 244, 6, 107, 180, 232, 211, 35, 152, 231, 130, 68, 126, 190, 190, 63, 247, 101, 54, 172 }), "Ensage", new Version(0, 0, 0, 1));
			PublicKeys.EnsageSharpSandbox1000 = new StrongName(new StrongNamePublicKeyBlob(new byte[] { 0, 36, 0, 0, 4, 128, 0, 0, 148, 0, 0, 0, 6, 2, 0, 0, 0, 36, 0, 0, 82, 83, 65, 49, 0, 4, 0, 0, 1, 0, 1, 0, 173, 74, 254, 121, 244, 14, 50, 196, 182, 250, 182, 48, 34, 81, 170, 47, 113, 198, 190, 70, 130, 32, 18, 220, 240, 89, 1, 108, 254, 112, 206, 136, 222, 6, 71, 219, 241, 15, 47, 238, 41, 149, 128, 124, 138, 178, 46, 206, 233, 24, 96, 177, 22, 13, 220, 234, 107, 246, 238, 31, 155, 227, 240, 182, 94, 117, 32, 170, 157, 130, 242, 113, 247, 7, 6, 54, 67, 122, 125, 207, 84, 147, 85, 99, 40, 125, 60, 130, 15, 24, 167, 104, 23, 168, 125, 62, 99, 152, 4, 43, 163, 125, 167, 95, 148, 206, 108, 66, 23, 243, 160, 8, 126, 186, 38, 202, 251, 149, 33, 176, 35, 43, 186, 55, 197, 232, 167, 202 }), "EnsageSharp.Sandbox", new Version(1, 0, 0, 0));
			PublicKeys.JetBrainsAnnotations10140 = new StrongName(new StrongNamePublicKeyBlob(new byte[] { 0, 36, 0, 0, 4, 128, 0, 0, 148, 0, 0, 0, 6, 2, 0, 0, 0, 36, 0, 0, 82, 83, 65, 49, 0, 4, 0, 0, 1, 0, 1, 0, 135, 246, 59, 166, 167, 137, 195, 14, 33, 14, 126, 201, 135, 35, 74, 217, 254, 51, 186, 247, 54, 121, 147, 186, 177, 179, 18, 214, 247, 44, 162, 150, 185, 30, 213, 198, 88, 150, 79, 251, 158, 117, 112, 235, 24, 74, 82, 124, 104, 198, 189, 186, 65, 207, 230, 125, 140, 253, 63, 136, 130, 52, 32, 107, 243, 146, 5, 163, 101, 45, 58, 243, 68, 91, 182, 247, 21, 253, 172, 83, 46, 40, 159, 234, 65, 34, 155, 172, 55, 118, 43, 103, 235, 22, 245, 143, 238, 113, 125, 36, 101, 252, 169, 238, 23, 240, 142, 209, 103, 114, 161, 252, 82, 193, 193, 112, 34, 225, 240, 217, 189, 208, 4, 82, 74, 102, 58, 202 }), "JetBrains.Annotations", new Version(10, 1, 4, 0));
			PublicKeys.JetBrainsAnnotations10150 = new StrongName(new StrongNamePublicKeyBlob(new byte[] { 0, 36, 0, 0, 4, 128, 0, 0, 148, 0, 0, 0, 6, 2, 0, 0, 0, 36, 0, 0, 82, 83, 65, 49, 0, 4, 0, 0, 1, 0, 1, 0, 135, 246, 59, 166, 167, 137, 195, 14, 33, 14, 126, 201, 135, 35, 74, 217, 254, 51, 186, 247, 54, 121, 147, 186, 177, 179, 18, 214, 247, 44, 162, 150, 185, 30, 213, 198, 88, 150, 79, 251, 158, 117, 112, 235, 24, 74, 82, 124, 104, 198, 189, 186, 65, 207, 230, 125, 140, 253, 63, 136, 130, 52, 32, 107, 243, 146, 5, 163, 101, 45, 58, 243, 68, 91, 182, 247, 21, 253, 172, 83, 46, 40, 159, 234, 65, 34, 155, 172, 55, 118, 43, 103, 235, 22, 245, 143, 238, 113, 125, 36, 101, 252, 169, 238, 23, 240, 142, 209, 103, 114, 161, 252, 82, 193, 193, 112, 34, 225, 240, 217, 189, 208, 4, 82, 74, 102, 58, 202 }), "JetBrains.Annotations", new Version(10, 1, 5, 0));
			PublicKeys.LeagueSharp10025 = new StrongName(new StrongNamePublicKeyBlob(new byte[] { 0, 36, 0, 0, 4, 128, 0, 0, 148, 0, 0, 0, 6, 2, 0, 0, 0, 36, 0, 0, 82, 83, 65, 49, 0, 4, 0, 0, 1, 0, 1, 0, 45, 195, 43, 160, 36, 244, 232, 29, 165, 81, 57, 166, 53, 127, 100, 30, 154, 164, 238, 1, 99, 113, 26, 142, 120, 161, 202, 56, 38, 58, 119, 105, 123, 13, 163, 182, 51, 46, 211, 188, 26, 65, 216, 172, 15, 247, 203, 28, 29, 84, 110, 232, 133, 25, 235, 232, 179, 247, 153, 201, 249, 121, 129, 106, 236, 101, 71, 120, 76, 228, 29, 16, 25, 11, 111, 71, 240, 150, 107, 135, 3, 223, 177, 17, 149, 197, 94, 240, 101, 76, 92, 216, 90, 97, 166, 180, 81, 73, 49, 64, 15, 225, 181, 1, 245, 226, 101, 79, 146, 244, 6, 107, 180, 232, 211, 35, 152, 231, 130, 68, 126, 190, 190, 63, 247, 101, 54, 172 }), "LeagueSharp", new Version(1, 0, 0, 25));
			PublicKeys.LeagueSharpSandbox1000 = new StrongName(new StrongNamePublicKeyBlob(new byte[] { 0, 36, 0, 0, 4, 128, 0, 0, 148, 0, 0, 0, 6, 2, 0, 0, 0, 36, 0, 0, 82, 83, 65, 49, 0, 4, 0, 0, 1, 0, 1, 0, 173, 74, 254, 121, 244, 14, 50, 196, 182, 250, 182, 48, 34, 81, 170, 47, 113, 198, 190, 70, 130, 32, 18, 220, 240, 89, 1, 108, 254, 112, 206, 136, 222, 6, 71, 219, 241, 15, 47, 238, 41, 149, 128, 124, 138, 178, 46, 206, 233, 24, 96, 177, 22, 13, 220, 234, 107, 246, 238, 31, 155, 227, 240, 182, 94, 117, 32, 170, 157, 130, 242, 113, 247, 7, 6, 54, 67, 122, 125, 207, 84, 147, 85, 99, 40, 125, 60, 130, 15, 24, 167, 104, 23, 168, 125, 62, 99, 152, 4, 43, 163, 125, 167, 95, 148, 206, 108, 66, 23, 243, 160, 8, 126, 186, 38, 202, 251, 149, 33, 176, 35, 43, 186, 55, 197, 232, 167, 202 }), "LeagueSharp.Sandbox", new Version(1, 0, 0, 0));
			PublicKeys.log4net12150 = new StrongName(new StrongNamePublicKeyBlob(new byte[] { 0, 36, 0, 0, 4, 128, 0, 0, 148, 0, 0, 0, 6, 2, 0, 0, 0, 36, 0, 0, 82, 83, 65, 49, 0, 4, 0, 0, 1, 0, 1, 0, 41, 125, 202, 201, 8, 226, 134, 137, 54, 3, 153, 2, 123, 14, 164, 205, 133, 47, 187, 116, 225, 237, 149, 230, 149, 165, 186, 85, 203, 209, 208, 117, 236, 32, 205, 181, 250, 111, 197, 147, 211, 213, 113, 82, 123, 32, 85, 141, 111, 57, 225, 244, 213, 207, 224, 121, 132, 40, 197, 137, 195, 17, 150, 82, 68, 178, 9, 195, 138, 2, 170, 168, 201, 218, 59, 114, 64, 91, 111, 237, 238, 180, 41, 44, 52, 87, 233, 118, 155, 116, 230, 69, 193, 156, 176, 108, 43, 231, 95, 178, 209, 34, 129, 165, 133, 251, 234, 191, 123, 209, 149, 214, 150, 27, 161, 19, 40, 111, 195, 226, 134, 215, 187, 214, 144, 36, 206, 218 }), "log4net", new Version(1, 2, 15, 0));
			PublicKeys.MonoCecil0960 = new StrongName(new StrongNamePublicKeyBlob(new byte[] { 0, 36, 0, 0, 4, 128, 0, 0, 148, 0, 0, 0, 6, 2, 0, 0, 0, 36, 0, 0, 82, 83, 65, 49, 0, 4, 0, 0, 1, 0, 1, 0, 121, 21, 153, 119, 210, 208, 58, 142, 107, 234, 122, 46, 116, 232, 209, 175, 204, 147, 232, 133, 25, 116, 149, 43, 180, 128, 161, 44, 145, 52, 71, 77, 4, 6, 36, 71, 195, 126, 14, 104, 192, 128, 83, 111, 207, 60, 63, 190, 47, 249, 201, 121, 206, 153, 132, 117, 229, 6, 232, 206, 130, 221, 91, 15, 53, 13, 193, 14, 147, 191, 46, 238, 207, 135, 75, 36, 119, 12, 80, 129, 219, 234, 116, 71, 253, 218, 250, 39, 123, 34, 222, 71, 214, 255, 234, 68, 150, 116, 164, 249, 252, 207, 132, 209, 80, 105, 8, 147, 128, 40, 77, 189, 211, 95, 70, 205, 255, 18, 161, 189, 120, 228, 239, 0, 101, 208, 22, 223 }), "Mono.Cecil", new Version(0, 9, 6, 0));
			PublicKeys.MonoSecurity4000 = new StrongName(new StrongNamePublicKeyBlob(new byte[] { 0, 36, 0, 0, 4, 128, 0, 0, 148, 0, 0, 0, 6, 2, 0, 0, 0, 36, 0, 0, 82, 83, 65, 49, 0, 4, 0, 0, 1, 0, 1, 0, 121, 21, 153, 119, 210, 208, 58, 142, 107, 234, 122, 46, 116, 232, 209, 175, 204, 147, 232, 133, 25, 116, 149, 43, 180, 128, 161, 44, 145, 52, 71, 77, 4, 6, 36, 71, 195, 126, 14, 104, 192, 128, 83, 111, 207, 60, 63, 190, 47, 249, 201, 121, 206, 153, 132, 117, 229, 6, 232, 206, 130, 221, 91, 15, 53, 13, 193, 14, 147, 191, 46, 238, 207, 135, 75, 36, 119, 12, 80, 129, 219, 234, 116, 71, 253, 218, 250, 39, 123, 34, 222, 71, 214, 255, 234, 68, 150, 116, 164, 249, 252, 207, 132, 209, 80, 105, 8, 147, 128, 40, 77, 189, 211, 95, 70, 205, 255, 18, 161, 189, 120, 228, 239, 0, 101, 208, 22, 223 }), "Mono.Security", new Version(4, 0, 0, 0));
			PublicKeys.NewtonsoftJson8000 = new StrongName(new StrongNamePublicKeyBlob(new byte[] { 0, 36, 0, 0, 4, 128, 0, 0, 148, 0, 0, 0, 6, 2, 0, 0, 0, 36, 0, 0, 82, 83, 65, 49, 0, 4, 0, 0, 1, 0, 1, 0, 245, 97, 223, 39, 124, 108, 11, 73, 125, 98, 144, 50, 180, 16, 205, 207, 40, 110, 83, 124, 5, 71, 36, 247, 255, 160, 22, 67, 69, 246, 43, 62, 100, 32, 41, 215, 168, 12, 195, 81, 145, 137, 85, 50, 140, 74, 220, 138, 4, 136, 35, 239, 144, 176, 207, 56, 234, 125, 176, 215, 41, 202, 242, 182, 51, 195, 186, 190, 8, 176, 49, 1, 152, 193, 8, 25, 149, 193, 144, 41, 188, 103, 81, 147, 116, 78, 171, 157, 115, 69, 184, 166, 114, 88, 236, 23, 209, 18, 206, 189, 187, 178, 162, 129, 72, 125, 206, 234, 251, 157, 131, 170, 147, 15, 50, 16, 63, 190, 29, 41, 17, 66, 91, 197, 116, 64, 2, 199 }), "Newtonsoft.Json", new Version(8, 0, 0, 0));
			PublicKeys.PlaySharpService1000 = new StrongName(new StrongNamePublicKeyBlob(new byte[] { 0, 36, 0, 0, 4, 128, 0, 0, 148, 0, 0, 0, 6, 2, 0, 0, 0, 36, 0, 0, 82, 83, 65, 49, 0, 4, 0, 0, 1, 0, 1, 0, 109, 71, 233, 12, 234, 206, 175, 225, 49, 227, 223, 39, 145, 140, 213, 189, 87, 101, 99, 206, 143, 18, 204, 66, 237, 15, 178, 228, 62, 240, 106, 93, 216, 214, 58, 214, 11, 68, 24, 175, 39, 10, 21, 15, 80, 0, 162, 89, 115, 195, 202, 166, 10, 181, 28, 145, 243, 185, 53, 75, 112, 105, 102, 14, 20, 192, 92, 57, 37, 251, 186, 46, 110, 241, 215, 121, 253, 170, 22, 151, 140, 94, 224, 254, 63, 210, 194, 240, 223, 117, 176, 122, 76, 80, 220, 65, 6, 159, 22, 231, 233, 45, 11, 236, 30, 128, 65, 38, 255, 183, 175, 187, 160, 41, 73, 23, 68, 151, 207, 71, 39, 143, 123, 100, 10, 184, 195, 185 }), "PlaySharp.Service", new Version(1, 0, 0, 0));
			PublicKeys.PlaySharpToolkit1000 = new StrongName(new StrongNamePublicKeyBlob(new byte[] { 0, 36, 0, 0, 4, 128, 0, 0, 148, 0, 0, 0, 6, 2, 0, 0, 0, 36, 0, 0, 82, 83, 65, 49, 0, 4, 0, 0, 1, 0, 1, 0, 109, 71, 233, 12, 234, 206, 175, 225, 49, 227, 223, 39, 145, 140, 213, 189, 87, 101, 99, 206, 143, 18, 204, 66, 237, 15, 178, 228, 62, 240, 106, 93, 216, 214, 58, 214, 11, 68, 24, 175, 39, 10, 21, 15, 80, 0, 162, 89, 115, 195, 202, 166, 10, 181, 28, 145, 243, 185, 53, 75, 112, 105, 102, 14, 20, 192, 92, 57, 37, 251, 186, 46, 110, 241, 215, 121, 253, 170, 22, 151, 140, 94, 224, 254, 63, 210, 194, 240, 223, 117, 176, 122, 76, 80, 220, 65, 6, 159, 22, 231, 233, 45, 11, 236, 30, 128, 65, 38, 255, 183, 175, 187, 160, 41, 73, 23, 68, 151, 207, 71, 39, 143, 123, 100, 10, 184, 195, 185 }), "PlaySharp.Toolkit", new Version(1, 0, 0, 0));
			PublicKeys.SharpDXDirect3D92630 = new StrongName(new StrongNamePublicKeyBlob(new byte[] { 0, 36, 0, 0, 4, 128, 0, 0, 148, 0, 0, 0, 6, 2, 0, 0, 0, 36, 0, 0, 82, 83, 65, 49, 0, 4, 0, 0, 1, 0, 1, 0, 77, 1, 219, 40, 66, 182, 80, 50, 155, 4, 67, 61, 71, 92, 242, 92, 24, 226, 109, 66, 63, 7, 81, 149, 109, 193, 11, 38, 173, 247, 239, 162, 137, 111, 44, 14, 48, 49, 53, 109, 165, 83, 99, 173, 145, 139, 111, 187, 5, 116, 91, 77, 187, 112, 51, 82, 253, 226, 79, 84, 241, 73, 170, 212, 180, 96, 251, 71, 203, 219, 223, 71, 56, 111, 22, 65, 18, 2, 79, 42, 130, 99, 11, 75, 170, 1, 16, 132, 177, 78, 47, 107, 117, 164, 54, 180, 84, 132, 151, 198, 167, 116, 202, 59, 175, 227, 205, 131, 71, 196, 32, 110, 213, 219, 247, 112, 223, 234, 253, 103, 199, 59, 67, 34, 89, 216, 246, 164 }), "SharpDX.Direct3D9", new Version(2, 6, 3, 0));
			PublicKeys.SharpDX2630 = new StrongName(new StrongNamePublicKeyBlob(new byte[] { 0, 36, 0, 0, 4, 128, 0, 0, 148, 0, 0, 0, 6, 2, 0, 0, 0, 36, 0, 0, 82, 83, 65, 49, 0, 4, 0, 0, 1, 0, 1, 0, 77, 1, 219, 40, 66, 182, 80, 50, 155, 4, 67, 61, 71, 92, 242, 92, 24, 226, 109, 66, 63, 7, 81, 149, 109, 193, 11, 38, 173, 247, 239, 162, 137, 111, 44, 14, 48, 49, 53, 109, 165, 83, 99, 173, 145, 139, 111, 187, 5, 116, 91, 77, 187, 112, 51, 82, 253, 226, 79, 84, 241, 73, 170, 212, 180, 96, 251, 71, 203, 219, 223, 71, 56, 111, 22, 65, 18, 2, 79, 42, 130, 99, 11, 75, 170, 1, 16, 132, 177, 78, 47, 107, 117, 164, 54, 180, 84, 132, 151, 198, 167, 116, 202, 59, 175, 227, 205, 131, 71, 196, 32, 110, 213, 219, 247, 112, 223, 234, 253, 103, 199, 59, 67, 34, 89, 216, 246, 164 }), "SharpDX", new Version(2, 6, 3, 0));
			PublicKeys.NewtonsoftJson9000 = new StrongName(new StrongNamePublicKeyBlob(new byte[] { 0, 36, 0, 0, 4, 128, 0, 0, 148, 0, 0, 0, 6, 2, 0, 0, 0, 36, 0, 0, 82, 83, 65, 49, 0, 4, 0, 0, 1, 0, 1, 0, 245, 97, 223, 39, 124, 108, 11, 73, 125, 98, 144, 50, 180, 16, 205, 207, 40, 110, 83, 124, 5, 71, 36, 247, 255, 160, 22, 67, 69, 246, 43, 62, 100, 32, 41, 215, 168, 12, 195, 81, 145, 137, 85, 50, 140, 74, 220, 138, 4, 136, 35, 239, 144, 176, 207, 56, 234, 125, 176, 215, 41, 202, 242, 182, 51, 195, 186, 190, 8, 176, 49, 1, 152, 193, 8, 25, 149, 193, 144, 41, 188, 103, 81, 147, 116, 78, 171, 157, 115, 69, 184, 166, 114, 88, 236, 23, 209, 18, 206, 189, 187, 178, 162, 129, 72, 125, 206, 234, 251, 157, 131, 170, 147, 15, 50, 16, 63, 190, 29, 41, 17, 66, 91, 197, 116, 64, 2, 199 }), "Newtonsoft.Json", new Version(9, 0, 0, 0));
			PublicKeys.MicrosoftVisualStudioThreading14000 = new StrongName(new StrongNamePublicKeyBlob(new byte[] { 0, 36, 0, 0, 4, 128, 0, 0, 148, 0, 0, 0, 6, 2, 0, 0, 0, 36, 0, 0, 82, 83, 65, 49, 0, 4, 0, 0, 1, 0, 1, 0, 7, 209, 250, 87, 196, 174, 217, 240, 163, 46, 132, 170, 15, 174, 253, 13, 233, 232, 253, 106, 236, 143, 135, 251, 3, 118, 108, 131, 76, 153, 146, 30, 178, 59, 231, 154, 217, 213, 220, 193, 221, 154, 210, 54, 19, 33, 2, 144, 11, 114, 60, 249, 128, 149, 127, 196, 225, 119, 16, 143, 198, 7, 119, 79, 41, 232, 50, 14, 146, 234, 5, 236, 228, 232, 33, 192, 165, 239, 232, 241, 100, 92, 76, 12, 147, 193, 171, 153, 40, 93, 98, 44, 170, 101, 44, 29, 250, 214, 61, 116, 93, 111, 45, 229, 241, 126, 94, 175, 15, 196, 150, 61, 38, 28, 138, 18, 67, 101, 24, 32, 109, 192, 147, 52, 77, 90, 210, 147 }), "Microsoft.VisualStudio.Threading", new Version(14, 0, 0, 0));
			PublicKeys.MicrosoftVisualStudioValidation14000 = new StrongName(new StrongNamePublicKeyBlob(new byte[] { 0, 36, 0, 0, 4, 128, 0, 0, 148, 0, 0, 0, 6, 2, 0, 0, 0, 36, 0, 0, 82, 83, 65, 49, 0, 4, 0, 0, 1, 0, 1, 0, 7, 209, 250, 87, 196, 174, 217, 240, 163, 46, 132, 170, 15, 174, 253, 13, 233, 232, 253, 106, 236, 143, 135, 251, 3, 118, 108, 131, 76, 153, 146, 30, 178, 59, 231, 154, 217, 213, 220, 193, 221, 154, 210, 54, 19, 33, 2, 144, 11, 114, 60, 249, 128, 149, 127, 196, 225, 119, 16, 143, 198, 7, 119, 79, 41, 232, 50, 14, 146, 234, 5, 236, 228, 232, 33, 192, 165, 239, 232, 241, 100, 92, 76, 12, 147, 193, 171, 153, 40, 93, 98, 44, 170, 101, 44, 29, 250, 214, 61, 116, 93, 111, 45, 229, 241, 126, 94, 175, 15, 196, 150, 61, 38, 28, 138, 18, 67, 101, 24, 32, 109, 192, 147, 52, 77, 90, 210, 147 }), "Microsoft.VisualStudio.Validation", new Version(14, 0, 0, 0));
			PublicKeys.SystemCollectionsImmutable1200 = new StrongName(new StrongNamePublicKeyBlob(new byte[] { 0, 36, 0, 0, 4, 128, 0, 0, 148, 0, 0, 0, 6, 2, 0, 0, 0, 36, 0, 0, 82, 83, 65, 49, 0, 4, 0, 0, 1, 0, 1, 0, 7, 209, 250, 87, 196, 174, 217, 240, 163, 46, 132, 170, 15, 174, 253, 13, 233, 232, 253, 106, 236, 143, 135, 251, 3, 118, 108, 131, 76, 153, 146, 30, 178, 59, 231, 154, 217, 213, 220, 193, 221, 154, 210, 54, 19, 33, 2, 144, 11, 114, 60, 249, 128, 149, 127, 196, 225, 119, 16, 143, 198, 7, 119, 79, 41, 232, 50, 14, 146, 234, 5, 236, 228, 232, 33, 192, 165, 239, 232, 241, 100, 92, 76, 12, 147, 193, 171, 153, 40, 93, 98, 44, 170, 101, 44, 29, 250, 214, 61, 116, 93, 111, 45, 229, 241, 126, 94, 175, 15, 196, 150, 61, 38, 28, 138, 18, 67, 101, 24, 32, 109, 192, 147, 52, 77, 90, 210, 147 }), "System.Collections.Immutable", new Version(1, 2, 0, 0));
			PublicKeys.PlaySharpAppDomainPluginLeagueSharp1000 = new StrongName(new StrongNamePublicKeyBlob(new byte[] { 0, 36, 0, 0, 4, 128, 0, 0, 148, 0, 0, 0, 6, 2, 0, 0, 0, 36, 0, 0, 82, 83, 65, 49, 0, 4, 0, 0, 1, 0, 1, 0, 109, 71, 233, 12, 234, 206, 175, 225, 49, 227, 223, 39, 145, 140, 213, 189, 87, 101, 99, 206, 143, 18, 204, 66, 237, 15, 178, 228, 62, 240, 106, 93, 216, 214, 58, 214, 11, 68, 24, 175, 39, 10, 21, 15, 80, 0, 162, 89, 115, 195, 202, 166, 10, 181, 28, 145, 243, 185, 53, 75, 112, 105, 102, 14, 20, 192, 92, 57, 37, 251, 186, 46, 110, 241, 215, 121, 253, 170, 22, 151, 140, 94, 224, 254, 63, 210, 194, 240, 223, 117, 176, 122, 76, 80, 220, 65, 6, 159, 22, 231, 233, 45, 11, 236, 30, 128, 65, 38, 255, 183, 175, 187, 160, 41, 73, 23, 68, 151, 207, 71, 39, 143, 123, 100, 10, 184, 195, 185 }), "PlaySharp.AppDomain.Plugin.LeagueSharp", new Version(1, 0, 0, 0));
			PublicKeys.Keys = new StrongName[] { PublicKeys.DotNetZip11010, PublicKeys.JetBrainsAnnotations10150, PublicKeys.JetBrainsAnnotations10140, PublicKeys.NewtonsoftJson9000, PublicKeys.NewtonsoftJson8000, PublicKeys.MicrosoftVisualStudioThreading14000, PublicKeys.MicrosoftVisualStudioValidation14000, PublicKeys.SystemCollectionsImmutable1200, PublicKeys.MonoCecil0960, PublicKeys.MonoSecurity4000, PublicKeys.log4net12150, PublicKeys.SharpDXDirect3D92630, PublicKeys.SharpDX2630, PublicKeys.PlaySharpService1000, PublicKeys.PlaySharpToolkit1000, PublicKeys.PlaySharpAppDomainPluginLeagueSharp1000, PublicKeys.Ensage0001, PublicKeys.EnsageSharpSandbox1000, PublicKeys.LeagueSharp10025, PublicKeys.LeagueSharpSandbox1000 };
		}

		public PublicKeys()
		{
		}
	}
}