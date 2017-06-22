open SDL

[<EntryPoint>]
let main argv = 

    let version =  Version.get()

    printf "SDL Version: %d.%d.%d\r\n" version.Major version.Minor version.Patch

    let revisionNumber = Version.getRevisionNumber()

    printf "Revision Number: %d\r\n" revisionNumber

    let revision = Version.getRevision()

    printf "Revision: %s\r\n" revision

    0
