/* License: NCSA Open Source License
 *
 * Copyright: SPT
 * AUTHORS:
 * Basuro
 */

namespace Spt.Core.Models;

public class PatchResult
{
    public PatchResult(PatchResultEnum resultEnum, byte[] patchedData)
    {
        ResultEnum = resultEnum;
        PatchedData = patchedData;
    }

    public PatchResultEnum ResultEnum
    {
        get;
    }

    public byte[] PatchedData
    {
        get;
    }
}
