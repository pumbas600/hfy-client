namespace HfyClientApi.Utils
{
  public abstract class Task
  {
    public abstract bool IsMergeableWith(Task other);
    public abstract void MergeWith(Task other);
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
    private readonly LinkedList<Task> _taskQueue = new();

    public EnqueuedTaskInfo Enqueue(Task task)
    {
      int totalCount;
      lock (_taskQueue)
      {
        totalCount = _taskQueue.Count;
        var index = totalCount - 1;
        var currentNode = _taskQueue.Last;

        while (currentNode != null)
        {
          if (currentNode.Value.IsMergeableWith(task))
          {
            currentNode.Value.MergeWith(task);
            return new EnqueuedTaskInfo
            {
              Index = index,
              EstimatedCompletionTimeMilliseconds = 0 // TODO: Calculate this
            };
          }

          index--;
          currentNode = currentNode.Previous;
        }

        _taskQueue.AddLast(task);
      }

      return new EnqueuedTaskInfo
      {
        Index = totalCount,
        EstimatedCompletionTimeMilliseconds = 0 // TODO: Calculate this
      };
    }
  }
}
