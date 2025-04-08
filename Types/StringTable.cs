using System;
using System.Collections.Generic;
using System.IO;
using Yarhl.IO;

namespace HActLib
{
    public class StringTable
    {
        private DataWriter m_writer;
        public long StartPos { get; private set; }
        private long m_curPos;

        private Dictionary<string, long> m_table = new();

        public int Count() { return m_table.Count; }

        public StringTable(DataWriter writer, long position)
        {
            StartPos = position;
            m_curPos = position;
            m_writer = writer;
        }

        public long Write(string value, bool allowDuplicate = false)
        {
            if (value == null)
                return 0;

            if (!allowDuplicate)
            {
                if (m_table.ContainsKey(value))
                    return m_table[value];
            }

            long addr = 0;

            m_writer.Stream.RunInPosition(delegate
            {
                addr = m_curPos;
                m_table[value] = addr;
                m_writer.Write(value);
                m_curPos = m_writer.Stream.Position;
            },m_curPos);

            return addr;
        }
    }
}
