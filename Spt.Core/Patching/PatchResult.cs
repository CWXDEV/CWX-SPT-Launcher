/* License: NCSA Open Source License
 *
 * Copyright: SPT
 * AUTHORS:
 * Basuro
 */

using Spt.Core.Enums;

namespace Spt.Core.Patching;

public class PatchResult
{
    public PatchResult(Enums.PatchResultEnum resultEnum, byte[] patchedData)
    {
        ResultEnum = resultEnum;
        PatchedData = patchedData;
    }

    public Enums.PatchResultEnum ResultEnum
    {
        get;
    }

    public byte[] PatchedData
    {
        get;
    }
}
