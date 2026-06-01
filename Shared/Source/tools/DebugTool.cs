using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using AVcontrol;

namespace Shared.Source.tools
{
    public class DebugTool : IAsyncDisposable
    {
        private Channel<req> requests = Channel.CreateUnbounded<req>();
        private Task executor;
        private CancellationTokenSource _cts = new();
        private FileStream fs;
        public DebugTool(string path)
        {
            fs = File.Create(path);
            executor = exec();
        }
        public void Log(string text)
        {
            var a = new req();
            a.content = text;
            a.type = req.Type.log;
            a.time = DateTime4b.Now.ToString();
            requests.Writer.TryWrite(a);
        }

        public void Error(string text)
        {
            var a = new req();
            a.content = text;
            a.type = req.Type.error;
            a.time = DateTime4b.Now.ToString();
            requests.Writer.TryWrite(a);
        }

        public void Warning(string text)
        {
            var a = new req();
            a.content = text;
            a.type = req.Type.warning;
            a.time = DateTime4b.Now.ToString();
            requests.Writer.TryWrite(a);
        }

        private async Task exec()
        {
            await foreach (var request in requests.Reader.ReadAllAsync(_cts.Token))
            {
                try
                {
                await fs.WriteAsync(ToBinary.Utf16($"{request.type.ToString()}({request.time}): {request.content}"));
                await fs.FlushAsync();
                }
                catch (OperationCanceledException)
                {
                    // ничего!
                }
                catch (Exception e)
                {
                    Log(e.ToString());
                }
            }
        }
        public async ValueTask DisposeAsync()
        {
            _cts.Cancel();
            await executor;
            fs.Close();
            requests.Writer.Complete();
        }

        private class req
        {
            public string content;
            public Type type;
            public string time;

            public enum Type{
                error,
                warning,
                log,
            }
        }
    }
}
