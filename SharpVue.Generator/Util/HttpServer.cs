using SharpVue.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpVue.Generator.Util
{
    public class HttpServer
    {
        private readonly HttpListener Listener;
        private string ContentRoot;

        public HttpServer(string contentRoot, int port = 0)
        {
            this.ContentRoot = contentRoot;

            if (port == 0)
                port = GetFreePort();

            this.Listener = new HttpListener();
            this.Listener.Prefixes.Add($"http://127.0.0.1:{port}/");
        }

        private static int GetFreePort()
        {
            var l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }

        public void Start()
        {
            Listener.Start();

            new Thread(Loop)
            {
                IsBackground = true,
                Name = "HTTP listener loop"
            }.Start();

            Logger.Info("HTTP server listening on " + Listener.Prefixes.First());
        }

        private void Loop()
        {
            while (Listener.IsListening)
            {
                var ctx = Listener.GetContext();

                Logger.Debug($"Got request: {ctx.Request.HttpMethod} {ctx.Request.RawUrl}");

                var filePath = Path.Combine(ContentRoot, ctx.Request.Url.LocalPath);

                if (File.Exists(filePath))
                {
                    using var file = File.OpenRead(filePath);
                    file.CopyTo(ctx.Response.OutputStream);
                }
                else
                {
                    ctx.Response.StatusCode = 404;
                }

                ctx.Response.Close();
            }
        }
    }
}
