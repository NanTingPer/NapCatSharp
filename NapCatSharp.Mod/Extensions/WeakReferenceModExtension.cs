namespace NapCatSharp.Mod.Extensions;

public static class WeakReferenceModExtension
{
    extension(WeakReference<NapCatSharp.Core.Mod> modWeakRef)
    {
        public string ModName
        {
            get
            {
                if(modWeakRef.TryGetTarget(out var mod)) {
                    return mod.ModName;
                }
                return string.Empty;
            }
        }
    }
}
