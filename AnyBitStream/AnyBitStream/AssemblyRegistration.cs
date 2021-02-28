using System.Runtime.CompilerServices;

// expose internals to test projects
[assembly: InternalsVisibleToAttribute("AnyBitStream.Tests")]
namespace AnyBitStream
{
    internal class AssemblyRegistration { }
}
