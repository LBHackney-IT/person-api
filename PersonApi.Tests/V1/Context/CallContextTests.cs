using AutoFixture;
using FluentAssertions;
using PersonApi.V1.Context;
using PersonApi.V1.Controllers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PersonApi.Tests.V1.Context
{
    public class CallContextTests
    {
        private int _contextChangedCount;
        private readonly Fixture _fixture = new Fixture();

        public CallContextTests()
        {
            CallContext.OnCurrentChanged += (s, e) => { _contextChangedCount++; };
        }

        [Fact]
        public void CallContextNoContextTest()
        {
            CallContext.Current.Should().BeNull();
        }

        [Fact]
        public void CallContextNewContextTest()
        {
            CallContext.NewContext();
            CallContext.Current.Should().NotBeNull();
            _contextChangedCount.Should().Be(1);
        }

        [Fact]
        public void CallContextClearContextTest()
        {
            CallContext.NewContext();
            CallContext.ClearCurrentContext();
            CallContext.Current.Should().BeNull();
            _contextChangedCount.Should().Be(2);
        }

        private async static Task DoStuff(ConcurrentDictionary<int, Guid?> threadsExecuted, int index)
        {
            await Task.Run(() => threadsExecuted[index] = CallContext.Current.CorrelationId).ConfigureAwait(false);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(20)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        public void CallContextUsesCorrectContextForThread(int threadCount)
        {
            List<Guid> allCorrelationIds = _fixture.CreateMany<Guid>(threadCount).ToList();
            ConcurrentDictionary<int, Guid?> threadsExecuted = new ConcurrentDictionary<int, Guid?>();
            Func<int, Task> act = async (int index) =>
            {
                await DoStuff(threadsExecuted, index).ConfigureAwait(false);
            };
            Func<int, Guid?, Task> threadTask = (int thisIndex, Guid? corellationId) =>
            {
                CallContext.NewContext();
                CallContext.Current.SetValue(Constants.CorrelationId, corellationId);
                return act(thisIndex);
            };

            List<Action> allActions = new List<Action>();
            for (int i = 0; i < threadCount; i++)
            {
                int thisIndex = i;
                allActions.Add(() => threadTask(thisIndex, allCorrelationIds[thisIndex]).GetAwaiter().GetResult());
            }
            Parallel.Invoke(allActions.ToArray());

            threadsExecuted.Values.Should().BeEquivalentTo(allCorrelationIds);
        }
    }
}
