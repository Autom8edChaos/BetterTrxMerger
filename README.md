# BetterTrxMerger
Merges two trx files from MSTest runs into one trx file. This trx file can be used to create SpecFlow reports with specflow.exe. Can works with other tooling, but not guaranteed.

Usage: `BetterTrxMerger receiving-trx-file donor-trx-file [output file]`

When output file is not defined, the data is merged into the receiving trx file.

The trx file that is created does contain all information for generating report files with the specflow.exe tool. Can result in undefined behaviour when used with other tooling because totals, start/end times etc. are not recalculated.

It is also possible to use the TrxMerger.cs class as standalone code. It extends the `XmlDocument` class. You can use it by loading trx files as XmlDocuments and apply the `.Merge(donorXmlDocument)` on this document to merge data into it.
