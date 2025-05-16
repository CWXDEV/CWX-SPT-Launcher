/* License: NCSA Open Source License
 *
 * Copyright: SPT
 * AUTHORS:
 * Basuro
 */

namespace Spt.Core.Models;

public enum PatchResultEnum
{
    Success,
    InputLengthMismatch,
    InputChecksumMismatch,
    AlreadyPatched,
    OutputChecksumMismatch
}
