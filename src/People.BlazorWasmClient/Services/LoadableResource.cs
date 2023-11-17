namespace People.BlazorWasmClient.Services;

public class LoadableResource
{
    public bool Loading { get; private set; } = false;
    public List<string> Errors { get; private set; } = new();

    public LoadableResource() { }

    public async Task TryInvoke(Func<Task> func)
    {
        Loading = true;
        Errors.Clear();

        try
        {
            await func();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
            Errors.Add(ex.Message);
        }
        finally
        {
            Loading = false;
        }
    }
}
