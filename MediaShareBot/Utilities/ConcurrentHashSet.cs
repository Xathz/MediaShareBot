using System;
using System.Collections.Generic;
using System.Threading;

namespace MediaShareBot.Utilities {

    /// <remarks>https://stackoverflow.com/a/18923091</remarks>
    public class ConcurrentHashSet<T> : IDisposable {

        private readonly ReaderWriterLockSlim _Lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        private readonly HashSet<T> _HashSet = new HashSet<T>();

        public int Count {
            get {
                _Lock.EnterReadLock();

                try {
                    return _HashSet.Count;
                } finally {
                    if (_Lock.IsReadLockHeld) {
                        _Lock.ExitReadLock();
                    }
                }
            }
        }

        public bool Add(T item) {
            _Lock.EnterWriteLock();

            try {
                return _HashSet.Add(item);
            } finally {
                if (_Lock.IsWriteLockHeld) {
                    _Lock.ExitWriteLock();
                }
            }
        }

        public bool Contains(T item) {
            _Lock.EnterReadLock();

            try {
                return _HashSet.Contains(item);
            } finally {
                if (_Lock.IsReadLockHeld) {
                    _Lock.ExitReadLock();
                }
            }
        }

        public bool Remove(T item) {
            _Lock.EnterWriteLock();

            try {
                return _HashSet.Remove(item);
            } finally {
                if (_Lock.IsWriteLockHeld) {
                    _Lock.ExitWriteLock();
                }
            }
        }

        public void Clear() {
            _Lock.EnterWriteLock();

            try {
                _HashSet.Clear();
            } finally {
                if (_Lock.IsWriteLockHeld) {
                    _Lock.ExitWriteLock();
                }
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                if (_Lock != null) {
                    _Lock.Dispose();
                }
            }
        }

        ~ConcurrentHashSet() {
            Dispose(false);
        }

    }

}
