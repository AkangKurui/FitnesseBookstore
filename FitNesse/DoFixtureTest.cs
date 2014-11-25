using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitNesse
{
    public class DoFixtureTest:fitlibrary.DoFixture
    {
        private String contents;
        public void FillTimesWith(int howmany, String what)
        {
            contents = "";
            for (int i = 0; i < howmany; i++)
            {
                contents = contents + what;
            }
        }
        public bool CharAtIs(int index, char c)
        {
            return contents[index] == c;
        }
        public void SetList(String[] strings)
        {
            contents = "";
            foreach (String s in strings)
            {
                contents = contents + s;
            }
        }
        //
        public char CharAt(int index)
        {
            return contents[index];
        }
    }
}
