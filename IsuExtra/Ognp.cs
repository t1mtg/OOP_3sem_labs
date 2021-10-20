using System;
using System.Collections.Generic;

namespace IsuExtra
{
    public class Ognp
    {
        public Ognp(string name,  uint numberOfStreams, uint maxAmountOfStudentsInStream)
        {
            Id = Guid.NewGuid();
            Name = name;
            Streams = new List<Stream>();
            for (int i = 0; i < numberOfStreams; i++)
            {
                Streams.Add(new Stream(maxAmountOfStudentsInStream));
            }
        }

        public Guid Id { get; }
        public string Name { get; }
        public List<Stream> Streams { get; }
    }
}