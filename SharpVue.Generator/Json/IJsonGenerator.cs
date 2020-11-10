using SharpVue.Loading;
using System.IO;

namespace SharpVue.Generator.Json
{
    public interface IJsonGenerator : IGenerator
    {
        void Generate(Workspace ws, Stream jsonData);
    }
}
