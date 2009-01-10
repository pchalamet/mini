/* Copyright (C) 2009 Samuel Fredrickson <kinghajj@gmail.com>
 * 
 * This file is part of Mini, an INI library for the .NET framework.
 *
 * Mini is free software: you can redistribute it and/or modify it under the
 * terms of the GNU Lesser General Public License as published by the Free
 * Software Foundation, either version 3 of the License, or (at your option) any
 * later version.
 *
 * Mini is distributed in the hope that it will be useful, but WITHOUT ANY
 * WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
 * A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more
 * details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with Mini. If not, see <http://www.gnu.org/licenses/>.
 */

/* IniSection.cs - Represents a section in an INI file.
 */

using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Mini
{
    public class IniSection : IniPart, IEnumerable<IniSetting>
    {
        internal IniFile ini;
        internal List<IniSetting> settings;

        #region Constructors
        /// <summary>
        /// Create a new INI section with the given name, comment, and file.
        /// </summary>
        /// <param name="name">
        /// The section's name.
        /// </param>
        /// <param name="comment">
        /// The section's comment.
        /// </param>
        /// <param name="ini">
        /// The file to which the section belongs.
        /// </param>
        internal IniSection(string name, string comment, IniFile ini)
        {
            this.ini = ini;
            settings = new List<IniSetting>();
            Comment = comment;
            Name = name;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Removes a section from its file.
        /// </summary>
        public void Remove()
        {
            ini.parts.Remove(this);
        }

        /// <summary>
        /// Writes the section to an output stream.
        /// </summary>
        /// <param name="writer">The stream to write to.</param>
        override internal void Write(StreamWriter writer)
        {
            new IniComment(Comment).Write(writer);
            writer.WriteLine("[{0}]", Name);
            foreach(var setting in this)
                setting.Write(writer);
            writer.WriteLine();
        }
        #endregion

        #region Enumerator
        /// <summary>
        /// Gets an iterator for the section's settings.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator`1"/>
        /// </returns>
        public IEnumerator<IniSetting> GetEnumerator()
        {
            foreach(IniSetting setting in settings)
                yield return setting;
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region Indexer
        /// <value>
        /// Gets a section's setting via its key.
        /// </value>
        public IniSetting this[string key]
        {
            get
            {
                IniSetting found = null;

                // try to find the section.
                foreach(IniSetting setting in this)
                    if(setting.Key.Equals(key))
                        found = setting;

                // if not, create it and add it.
                if(found == null)
                {
                    found = new IniSetting(key, string.Empty, this);
                    settings.Add(found);
                }

                return found;
            }
        }

        /// <summary>
        /// Gets a section's setting via its index.
        /// </summary>
        public IniSetting this[int index]
        {
            get
            {
                return settings[index];
            }
        }
        #endregion

        #region Properties
        /// <value>
        /// Get or set a section's comment.
        /// </value>
        public string Comment
        {
            get;
            set;
        }

        /// <value>
        /// Get or set a section's name.
        /// </value>
        public string Name { get; set; }

        /// <value>
        /// Get a section's file.
        /// </value>
        public IniFile Ini { get { return ini; } }
        #endregion
    }
}
