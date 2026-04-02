[한국어](README.ko.md)

# AutoHwp2Pdf

AutoHwp2Pdf watches a folder and automatically converts HWP files into PDF files. It is meant for people who regularly receive Hancom Hangul documents and want PDFs to be created without repeating the same export steps by hand.

## Before You Start

- Use it on Windows.
- Make sure Hancom Hangul is installed and can open your HWP files.
- Prepare the folder you want to watch and the folder where you want PDFs to be saved.

## Quick Start

1. Install or open AutoHwp2Pdf and launch the app.
2. Set `Watch folder` to the folder where HWP files will arrive.
3. Choose a saving method in `PDF output mode`.
4. If you choose `Custom root folder`, set `Output root`.
5. If the app asks about the security module, check the path and click `Register`.
6. Click `Save Settings`.
7. Leave the app running. New HWP files in the watch folder will be converted automatically.

## PDF Saving Options

- `Same folder as source`: saves the PDF next to the original HWP file.
- `Child folder under source`: creates a subfolder and saves PDFs there.
- `Custom root folder`: sends PDFs to one separate folder that you choose.

## Everyday Use

- `Scan Now` checks the current watch folder immediately.
- `Pause Watching` temporarily stops automatic conversion.
- `Resume Watching` starts monitoring again.
- The app can start with Windows and continue running in the system tray.
- If a PDF is already up to date, the app skips it.

## If PDFs Are Not Being Created

- Make sure the HWP file opens normally in Hancom Hangul.
- Check that `Watch folder` and `Output root` point to real folders.
- Look at `Security status` and use `Register` if needed.
- Open `Logs` to see what happened.
