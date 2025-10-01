# Supported .NET Frameworks

As of v0.7.0, the OpenFGA .NET SDK ships as a multi-targeted NuGet package to balance broad compatibility with modern runtime features.

| Target | Status (at v0.7.x release) | Rationale | Notes |
| ------ | -------------------------- | --------- | ----- |
| `net8.0` | LTS (Active) | Primary target | Full feature set, implicit usings enabled. Recommended for new apps. |
| `net9.0` | Current (Non‑LTS) | Early adopter / perf | Falls back to `net8.0` behaviors where APIs identical. Will be replaced by future LTS when available. |
| `netstandard2.0` | Compatibility | Broad legacy reach (Core 2.0+, .NET 5/6/7, Framework ≥4.6.1, Xamarin/Unity) | Used automatically by `net6.0`/`net7.0` projects. Nullable annotations included. |
| `net48` | Legacy Full Framework | Enterprise / classic ASP.NET | Adds binding redirects & `System.Web` reference where needed. |

## Support Policy

We aim to follow the official Microsoft .NET support lifecycle:
https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core

Implications:

1. Frameworks still under Microsoft support (LTS or STS) receive compatibility testing in CI and issues affecting them are prioritized.
2. Runtimes past their end-of-support (EOL) may continue to work through the `netstandard2.0` asset, but:
   * They are treated as "best effort".
   * Issues specific to EOL frameworks may be closed if a modern supported runtime path exists.
   * We may remove explicit tests or even drop a target without a major (SemVer) version bump if Microsoft no longer supports it. (Removing a target that only affects unsupported runtimes is considered non-breaking for this SDK.)

## CI / Test Matrix

Current workflow jobs (see [`.github/workflows/main.yaml`](.github/workflows/main.yaml)) exercise:

| Framework | Platform(s) | Purpose | Support Status |
| --------- | ----------- | ------- | -------------- |
| `net8.0` | Ubuntu, Windows, macOS | Primary validation | Full support |
| `net9.0` | Ubuntu | Forward compatibility sanity | Full support |
| `net48` | Windows | Legacy full framework behavior | Full support |
| `net6.0` | Ubuntu | Validates consumption of the `netstandard2.0` build on a still-supported (if in maintenance) runtime | Maintenance support |
| `netcoreapp3.1` | Windows | EOL: ONLY to detect accidental regressions of the `netstandard2.0` surface; not a commitment of security/bug fix support | Best effort only |

> Note: You may not see a separate `net6.0` (or `net7.0`) target in the DLL list because those runtimes resolve the `netstandard2.0` asset.

## Dropping / Adding Targets

We periodically reevaluate targets based on:

* Microsoft lifecycle phase (Active / Maintenance / EOL)
* Package size impact
* Build/test cost
* Reported issues unique to a target

Criteria for removal without major bump (non-breaking):

* Target is EOL per Microsoft OR
* A supported runtime can consume an alternative asset (e.g., via `netstandard2.0`).

We will call out removals in `CHANGELOG.md`.

## Troubleshooting Matrix Issues

<!--
  As new issues come in that can be resolved by the user, add them to this table.
-->

| Symptom | Likely Cause | Mitigation |
| ------- | ------------ | ---------- |
| Only one DLL in `bin` after install | NuGet selected the best TFM | Expected behavior; nothing to fix. |
| Missing `System.Text.Json` on older project | Implicit dependency not restored | Update SDK / ensure `Restore` succeeded; if still failing, add explicit reference temporarily and open an issue. |

## Filing Framework-Specific Issues

Please include:

* Output of `dotnet --info` (or environment details for full framework)
* Your project target framework(s)
* `OpenFga.Sdk` nuget version
* Minimal reproduction (ideally a single .csproj + Program.cs)

## FAQ

**Why no explicit `net6.0` or `net7.0` target?**  
`netstandard2.0` already provides the necessary API surface. Adding additional identical builds increases package size and CI time without functional gain.

**Will you add future LTS (e.g., net10.0) on release?**  
Yes, typically shortly after GA, once CI images are available.

**Can support be guaranteed for EOL frameworks?**  
No. Those frameworks may work incidentally; we may remove related shims at any time.
