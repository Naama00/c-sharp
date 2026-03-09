# Contribution Guidelines and Coding Standards

## Purpose
This document records the project-wide coding standards for Business Objects (BO) and related contributions. These standards are mandatory and must be followed when adding or modifying BO classes.

## Directory structure
- All BO entities must be placed under the `BL/BO` folder.
- Each entity shall be implemented as one public class in its own file. File name must match the class name and use the `.cs` extension (for example, `Product.cs`).

## Naming and accessibility
- Class names must be PascalCase and match the entity name exactly (for example: `Product`, `Customer`, `Sale`).
- All BO classes must have `public` accessibility.

## Properties
- Use public auto-properties for all data members. Do not add public or private fields.
- Think carefully about nullability for each property and annotate with C# nullable reference types where appropriate.
- Properties that should be immutable after construction (for example: `Id`) must use `init` accessors (read-only after initialization).
- Do not add methods to BO classes except an override of `ToString()` (override is allowed but the project will populate a reflection-based implementation later).

## Enums
- Any property that is an enum must use a corresponding enum in `BL/BO/Enums.cs` (the BO enum types must be defined there).

## Relationship with DO layer
- BO classes must be designed to mirror the DAL/DO definitions for shape and types. Use the existing DO records as authoritative for which properties and types are required.

## Formatting and style
- Follow the repository's `.editorconfig` rules (created if missing) for indentation, naming and file formatting.
- Keep classes minimal: no helper methods, no business logic inside BO classes.

## Example checklist for a new BO class
- File created in `BL/BO/<EntityName>.cs`.
- Public class named `<EntityName>`.
- All properties are public auto-properties.
- `Id` (or other identity fields) declared with `init` to be read-only after construction.
- Enum properties refer to types in `BL/BO/Enums.cs`.
- No methods added except optional `ToString()` override.

## Enforcement
- Contributors and reviewers must ensure these rules during code review.
- If any rule must be broken, document the rationale in the pull request description.

---

These guidelines will be merged into the repository's CONTRIBUTING.md file to ensure consistency in how BO entities are implemented across the project.