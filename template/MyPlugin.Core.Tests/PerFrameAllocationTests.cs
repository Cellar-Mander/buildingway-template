using MyPlugin.Core;

namespace MyPlugin.Core.Tests;

/// <summary>
/// Per-frame code that allocates makes the garbage collector pause the game. Each test here runs a
/// per-frame method like the game would and checks it allocates nothing; see "Per-frame code" in
/// CONTRIBUTING.md.
/// </summary>
public class PerFrameAllocationTests
{
    // The first runs allocate while the JIT compiles the code, so they don't count
    private const int WarmUpRuns = 3;
    private const int Frames = 100_000;

    [Fact]
    public void SaveDebouncerDoesNotAllocatePerFrame()
    {
        var debouncer = new SaveDebouncer(500);

        AssertNoAllocation(() => SimulateDragging(debouncer));
    }

    // Changes every 100 frames at 60 fps, so both the waiting and the saving branches run
    private static int SimulateDragging(SaveDebouncer debouncer)
    {
        var saves = 0;
        for (long frame = 0; frame < Frames; frame++)
        {
            var nowMs = frame * 16;
            if (frame % 100 == 0)
            {
                debouncer.Changed(nowMs);
            }

            if (debouncer.ShouldSave(nowMs))
            {
                saves++;
            }
        }

        return saves;
    }

    private static void AssertNoAllocation(Func<int> runFrames)
    {
        for (var i = 0; i < WarmUpRuns; i++)
        {
            runFrames();
        }

        var before = GC.GetAllocatedBytesForCurrentThread();
        var result = runFrames();
        var allocated = GC.GetAllocatedBytesForCurrentThread() - before;

        Assert.True(result > 0, "the measured frames should reach the code under test");
        Assert.Equal(0, allocated);
    }
}
