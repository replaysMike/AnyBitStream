# AnyBitStream

[![nuget](https://img.shields.io/nuget/v/AnyBitStream.svg)](https://www.nuget.org/packages/AnyBitStream/)
[![nuget](https://img.shields.io/nuget/dt/AnyBitStream.svg)](https://www.nuget.org/packages/AnyBitStream/)
[![Build status](https://ci.appveyor.com/api/projects/status/gfwjabg1pta7em94?svg=true)](https://ci.appveyor.com/project/MichaelBrown/anybitstream)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/977cab1b5c7e4d7ca749c691c6066a8f)](https://www.codacy.com/gh/replaysMike/AnyBitStream/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=replaysMike/AnyBitStream&amp;utm_campaign=Badge_Grade)

Work with bits efficiently in a stream using standard streams and extending BinaryReader/BinaryWriter.

## Description

AnyBitStream is an efficient stream based class for working with data at the bit level. It supports non-standard numeric types such as `Int2` to `Int7`, `Int10,Int12,Int24,Int48` and is perfect for working with network protocols or general bit-packing operations.

## Installation
Install AnyBitStream from the Package Manager Console:
```
PM> Install-Package AnyBitStream
```

## Usage

```csharp
using AnyBitStream;

var buffer = new byte[64] { // some byte data };
var stream = new BitStream(buffer);
var reader = new BitStreamReader(stream);

// read a 12 byte header
var header = new MyHeader {
  Marker = reader.ReadInt4(),
  ContinuityId = reader.ReadInt3(),
  IsValid = reader.ReadBit(),
  Length = reader.ReadInt32()
};
// read in some bytes
var data = reader.ReadBytes(header.Length);
```

### Unaligned vs Aligned bytes

By default, the `BitStream` will not allow unaligned data to be read/written. For example, if you've read 5 bits and then try to read a byte an exception will be thrown. Because you tried to read a byte that doesn't reside on a byte boundary (8 bits) it doesn't want to give you potentially bad data. However sometimes this behavior is desired with certain bit packing formats. 

To enable reading/writing unaligned data:
```csharp
// via constructor(s) on the stream
var stream = new BitStream(true);
var stream = new BitStream(buffer, writable: true, allowUnalignedOperations: true);
// via property
stream.AllowUnalignedOperations = true;
// also available via the reader/writers
var reader = new BitStreamReader(stream, leaveOpen: true, allowUnalignedOperations: true);
var writer = new BitStreamWriter(stream, leaveOpen: true, allowUnalignedOperations: true);
```

See more on reading unaligned streams in the wiki.

