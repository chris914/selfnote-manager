using NoteToSelf.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NoteToSelf
{
    public class NotesManager
    {
        private static NotesManager instance;
        public static NotesManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NotesManager();
                    instance.notes = new List<Note>();
                    instance.dataFile = Path.Combine(instance.baseFolder, "data.json");
                }

                return instance;
            }
        }

        private List<Note> notes;
        private string baseFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Note To Self");
        private string dataFile;

        public void LoadNotes()
        {
            if (!Directory.Exists(baseFolder))
                return;

            if (!File.Exists(dataFile))
                return;

            using (StreamReader sr = new StreamReader(dataFile))
            {
                var jsonString = sr.ReadLine();
                if (string.IsNullOrEmpty(jsonString))
                    return;

                notes = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Note>>(jsonString);
            }
        }

        internal void EditNote(Note note)
        {
            // ?
        }

        internal void AddNote(Note note)
        {
            notes.Add(note);
        }

        internal List<Note> GetNotes(string filterString = null)
        {
            if (string.IsNullOrEmpty(filterString))
                return notes.OrderByDescending(x => x.LastModified).ToList();

            var fs = filterString.ToLower();
            return notes.Where(x => (x.Title != null && x.Title.ToLower().Contains(fs))
            || (x.Description != null && x.Description.ToLower().Contains(fs))
            || (x.Tags != null && x.Tags.Count(y => y.ToLower().Contains(fs)) > 0)).ToList();
        }

        internal void DeleteNote(Note note)
        {
            notes.Remove(note);
        }

        public void SaveNotes()
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(notes);

            if (!Directory.Exists(baseFolder))
                Directory.CreateDirectory(baseFolder);

            using (StreamWriter sw = new StreamWriter(dataFile))
            {
                sw.WriteLine(jsonString);
            }
        }
    }
}
