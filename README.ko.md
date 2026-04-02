# AutoHwp2Pdf

AutoHwp2Pdf는 지정한 폴더를 지켜보다가 HWP 파일이 들어오면 자동으로 PDF로 바꿔 주는 프로그램입니다. 한컴 한글 문서를 자주 받아서 PDF를 반복해서 만들어야 할 때 편하게 사용할 수 있습니다.

## 시작 전 준비

- Windows에서 사용합니다.
- 한컴 한글이 설치되어 있고 HWP 파일이 정상적으로 열려야 합니다.
- 감시할 폴더와 PDF를 저장할 폴더를 미리 정해 두면 편합니다.

## 빠른 사용법

1. AutoHwp2Pdf를 설치하거나 실행합니다.
2. `Watch folder`에 HWP 파일이 들어오는 폴더를 지정합니다.
3. `PDF output mode`에서 저장 방식을 고릅니다.
4. `Custom root folder`를 선택했다면 `Output root`도 지정합니다.
5. 보안 모듈 등록 안내가 나오면 경로를 확인한 뒤 `Register`를 누릅니다.
6. `Save Settings`를 눌러 저장합니다.
7. 프로그램을 켜 둡니다. 감시 폴더에 새 HWP 파일이 들어오면 자동으로 PDF가 만들어집니다.

## PDF 저장 방식

- `Same folder as source`: 원본 HWP 파일 옆에 PDF를 저장합니다.
- `Child folder under source`: 원본 폴더 아래에 하위 폴더를 만들어 PDF를 저장합니다.
- `Custom root folder`: 사용자가 정한 별도 폴더로 PDF를 모아서 저장합니다.

## 평소 사용할 때

- `Scan Now`를 누르면 현재 감시 폴더를 바로 다시 확인합니다.
- `Pause Watching`을 누르면 자동 변환을 잠시 멈춥니다.
- `Resume Watching`을 누르면 다시 감시를 시작합니다.
- Windows 시작 시 자동 실행되게 둘 수 있고, 시스템 트레이에서 계속 동작할 수 있습니다.
- 이미 최신 PDF가 있으면 다시 만들지 않고 건너뜁니다.

## PDF가 만들어지지 않을 때

- HWP 파일이 한컴 한글에서 정상적으로 열리는지 확인합니다.
- `Watch folder`와 `Output root`가 실제 폴더를 가리키는지 확인합니다.
- `Security status`를 보고 필요하면 `Register`를 눌러 등록합니다.
- `Logs`를 열어 어떤 문제가 있었는지 확인합니다.
