﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFormatReader.Common;
using OpenTK;

namespace WindEditor.Events
{
    public class SubstanceData
    {
        public List<int> IntegerBank { get; private set; }
        public List<float> FloatBank { get; private set; }
        public List<char> CharBank { get; private set; }

        public SubstanceData(EndianBinaryReader reader)
        {
            IntegerBank = new List<int>();
            FloatBank = new List<float>();
            CharBank = new List<char>();

            int float_offset = reader.ReadInt32At(32);
            int float_count = reader.ReadInt32At(36);

            int integer_offset = reader.ReadInt32At(40);
            int integer_count = reader.ReadInt32At(44);

            int char_offset = reader.ReadInt32At(48);
            int char_count = reader.ReadInt32At(52);

            reader.BaseStream.Seek(float_offset, System.IO.SeekOrigin.Begin);
            for (int i = 0; i < float_count; i++)
            {
                FloatBank.Add(reader.ReadSingle());
            }

            reader.BaseStream.Seek(integer_offset, System.IO.SeekOrigin.Begin);
            for (int i = 0; i < integer_count; i++)
            {
                IntegerBank.Add(reader.ReadInt32());
            }

            reader.BaseStream.Seek(char_offset, System.IO.SeekOrigin.Begin);
            CharBank.AddRange(reader.ReadChars(char_count));

            reader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
        }

        public IntWrapper[] GetIntData(int starting_index, int count)
        {
            IntWrapper[] wrap_data = new IntWrapper[count];

            for (int i = 0; i < count; i++)
            {
                wrap_data[i] = new IntWrapper(IntegerBank[starting_index + i]);
            }

            return wrap_data;
        }

        public int AddIntData(IntWrapper[] int_data)
        {
            int new_offset = IntegerBank.Count;

            for (int i = 0; i < int_data.Length; i++)
            {
                IntegerBank.Add(int_data[i].IntValue);
            }

            return new_offset;
        }

        public FloatWrapper[] GetFloatData(int starting_index, int count)
        {
            FloatWrapper[] wrap_data = new FloatWrapper[count];

            for (int i = 0; i < count; i++)
            {
                wrap_data[i] = new FloatWrapper(FloatBank[starting_index + i]);
            }

            return wrap_data;
        }

        public int AddFloatData(FloatWrapper[] float_data)
        {
            int new_offset = FloatBank.Count;

            for (int i = 0; i < float_data.Length; i++)
            {
                FloatBank.Add(float_data[i].FloatValue);
            }

            return new_offset;
        }

        public Vector3[] GetVec3Data(int starting_index, int count)
        {
            Vector3[] data = new Vector3[count];

            for (int i = 0; i < count; i++)
            {
                int base_index = starting_index + (i * 3);
                data[i] = new Vector3(FloatBank[base_index], FloatBank[base_index + 1], FloatBank[base_index + 2]);
            }

            return data;
        }

        public int AddVec3Data(Vector3[] vec3_data)
        {
            int new_offset = FloatBank.Count;

            foreach (Vector3 v in vec3_data)
            {
                FloatBank.Add(v.X);
                FloatBank.Add(v.Y);
                FloatBank.Add(v.Z);
            }

            return new_offset;
        }

        public string GetStringData(int starting_index, int count)
        {
            char[] data = new char[count];
            CharBank.CopyTo(starting_index, data, 0, count);

            return new string(data);
        }

        public int AddStringData(string string_data)
        {
            int new_offset = CharBank.Count;

            CharBank.AddRange(Encoding.ASCII.GetChars(Encoding.ASCII.GetBytes(string_data)));

            return new_offset;
        }
    }
}