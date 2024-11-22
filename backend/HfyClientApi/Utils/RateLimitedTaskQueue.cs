using System.Collections.Concurrent;

namespace HfyClientApi.Utils
{
  public interface Task { }

  public abstract class MergeableTask : Task
  {
    public abstract bool TryMergeWith(MergeableTask other);
  }

  public class EnqueuedTaskInfo
  {
    /// <summary>
    /// A zero-based index of the task in the queue.
    /// </summary>
    public required int Index { get; set; }

    /// <summary>
    /// The estimated time in milliseconds that the task will take to complete based on previous
    /// tasks.
    /// </summary>
    public required int EstimatedCompletionTimeMilliseconds { get; set; }
  }

  public class RateLimitedTaskQueue
  {
    private readonly ConcurrentQueue<Task> _scheduledTasks = new();
    private readonly ConcurrentQueue<MergeableTask> _mergeableTasks = new();

    public EnqueuedTaskInfo Enqueue(Task task)
    {
      if (task is not MergeableTask mergeableTask)
      {
        _scheduledTasks.Enqueue(task);
      }
      else
      {
        _mergeableTasks.Enqueue(mergeableTask);
      }
    }
  }
}
