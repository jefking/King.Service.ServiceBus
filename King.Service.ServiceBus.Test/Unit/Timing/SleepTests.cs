namespace King.Service.ServiceBus.Unit.Tests.Timing
{
    using King.Service.ServiceBus.Timing;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    
    [TestFixture]
    public class SleepTests
    {
        [Test]
        public void Constructor()
        {
            new Sleep();
        }

        [Test]
        public void IsISleep()
        {
            Assert.IsNotNull(new Sleep() as ISleep);
        }

        [Test]
        public void Until()
        {
            var now = DateTime.UtcNow;
            var s = new Sleep();
            s.Until(DateTime.UtcNow.AddMilliseconds(251));
            Assert.IsTrue(now.AddMilliseconds(250) < DateTime.UtcNow);
        }

        [Test]
        public void UntilPast()
        {
            var s = new Sleep();
            s.Until(DateTime.UtcNow.AddMilliseconds(-251));
        }
    }
}