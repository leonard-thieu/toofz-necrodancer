using System.Collections.ObjectModel;

namespace toofz.NecroDancer.Dungeons
{
    /// <summary>
    /// Synchronizes <see cref="Level.Id"/> with its index in this collection.
    /// </summary>
    public sealed class LevelCollection : ObservableCollection<Level>
    {
        protected override void InsertItem(int index, Level item)
        {
            base.InsertItem(index, item);
            UpdateLevelIds();
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            base.MoveItem(oldIndex, newIndex);
            UpdateLevelIds();
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            UpdateLevelIds();
        }

        protected override void SetItem(int index, Level item)
        {
            base.SetItem(index, item);
            UpdateLevelIds();
        }

        void UpdateLevelIds()
        {
            for (int i = 0; i < Count; i++)
                Items[i].Id = i + 1;
        }
    }
}
