/* License: NCSA Open Source License
 *
 * Copyright: SPT
 * AUTHORS:
 * Basuro
 */

using Spt.Core.Enums;

namespace Spt.Core.Patching;

public class DiffResult
{
    public DiffResult(Enums.DiffResultEnum resultEnum, PatchInfo patchInfo)
    {
        ResultEnum = resultEnum;
        PatchInfo = patchInfo;
    }

    public Enums.DiffResultEnum ResultEnum
    {
        get;
    }

    public PatchInfo PatchInfo
    {
        get;
    }
}
