using System.Collections.Generic;
using Xunit;

namespace SyslogNet.Client.Tests
{
	public class StructuredDataElementTests
	{
		[Fact]
		public void StructuredDataIdWillAlwaysBePrependedWithAPrivateEnterpriseNumber()
		{
			var parameters = new Dictionary<string, string>
			{
				{ "key1", "aaa" }
			};
			var sut = new StructuredDataElement("SomeSdId", parameters);

#pragma warning disable 612
			Assert.Equal("SomeSdId@" + StructuredDataElement.DefaultPrivateEnterpriseNumber, sut.SdId);
#pragma warning restore 612
        }

		[Fact]
		public void StructuredDataIdWillRetainProvidedPrivateEnterpriseNumber()
		{
			var parameters = new Dictionary<string, string>
			{
				{ "key1", "aaa" }
			};
			var sut = new StructuredDataElement("SomeSdId@12345", parameters);

			Assert.Equal("SomeSdId@12345", sut.SdId);
		}
	}
}