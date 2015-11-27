using UnityEngine;
using System.Collections;

namespace ItemFramework.Db {
    public interface IDbHandler {
        void Load();
        void Save();
    }
}