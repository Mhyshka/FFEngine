using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;

namespace FF
{
    internal class FFVersion : IByteStreamSerialized
    {
        protected static string VERSION_PATTERN = @"^\d+.\d+.\d+$";
        protected static Regex VERSION_REGEX = new Regex(VERSION_PATTERN);

        protected int major = 0;
        protected int minor = 0;
        protected int patch = 0;

        public FFVersion()
        {
        }

        internal FFVersion(int a_major, int a_minor, int a_patch)
        {
            major = a_major;
            minor = a_minor;
            patch = a_patch;
        }

        internal FFVersion(string a_version)
        {
            if (VERSION_REGEX.IsMatch(a_version))
            {
                string[] split = a_version.Split('.');

                major = int.Parse(split[0]);
                minor = int.Parse(split[1]);
                patch = int.Parse(split[2]);
            }
        }

        #region Object
        public override string ToString()
        {
            return major.ToString() + "." + minor.ToString() + "." + patch.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is FFVersion)
            {
                FFVersion other = obj as FFVersion;
                return major == other.major &&
                        minor == other.minor &&
                        patch == other.patch;
            }
            else
                return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
        #endregion
        
        public void SerializeData(FFByteWriter stream)
        {
            stream.Write(major);
            stream.Write(minor);
            stream.Write(patch);
        }

        public void LoadFromData(FFByteReader stream)
        {
            major = stream.TryReadInt();
            minor = stream.TryReadInt();
            patch = stream.TryReadInt();
        }
    }
}