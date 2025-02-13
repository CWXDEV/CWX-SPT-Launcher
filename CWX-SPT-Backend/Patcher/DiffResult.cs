/* License: NCSA Open Source License
 *
 * Copyright: SPT
 * AUTHORS:
 * Basuro
 */

using CWX_SPT_Launcher_Backend.Patcher.Enums;

namespace CWX_SPT_Launcher_Backend.Patcher;

public class DiffResult
{
    public DiffResult(EDiffResultType result, PatchInfo patchInfo)
    {
        Result = result;
        PatchInfo = patchInfo;
    }

    public EDiffResultType Result
    {
        get;
    }

    public PatchInfo PatchInfo
    {
        get;
    }
}
