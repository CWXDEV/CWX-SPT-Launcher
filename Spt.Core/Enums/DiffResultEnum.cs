/* License: NCSA Open Source License
 *
 * Copyright: SPT
 * AUTHORS:
 * Basuro
 */

namespace Spt.Core.Enums;

public enum DiffResultEnum
{
    Success,
    OriginalFilePathInvalid,
    OriginalFileNotFound,
    OriginalFileReadFailed,
    PatchedFilePathInvalid,
    PatchedFileNotFound,
    PatchedFileReadFailed,
    FilesMatch
}
