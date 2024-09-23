# 🦙 Lazy VPM Package Template

A simple VPM package template with **non-unity-structure** and GitHub Action workflows for managing releases: creating, updating, and testing.

## ▶ Getting Started

1. **Generate Template**  
   [![Use this template](https://img.shields.io/badge/🌟%20Generate%20Template-Click%20Here-%234fc3f7?style=flat-square&logo=github&logoColor=white)](https://github.com/SweetForest/LazyVPMPackageTemplate/generate)

2. **Set Workflow Permissions**  
   After generating the repository, go to your repository's settings and enable workflows under `Settings > Actions > General`. Look at **Workflow permissions** and give **Read and write permissions.** 🔒

3. **Upload Your Files**  
   Replace the template files with your own project files. and push it on github. Make sure you still keep folder .github/workflows for automated release management. 📂

## 📦 Publishing a Release

The release process is managed through the `release.yml` file in the `/actions/workflows/` directory. The workflow offers three modes of operation: **Create**, **Update**, and **Test**. When triggering a workflow manually, you can choose one of these options:

- **Create**:  
  This will create a public release. 🌍 Before releasing, it checks if the version you're about to release already exists. If it does, the release process will abort to avoid duplicates. ❌

- **Update**:  
  This mode will also create a public release, but if a release with the same version already exists, it will automatically delete the existing release and create a new one. 🔄

- **Test**:  
  This mode will create a **draft** release, allowing you to test the release process without making it public. 📝

Once you have set everything up, you can trigger the workflow from the **Actions** tab in your GitHub repository. Select the **release.yml** workflow, click the "Run workflow" button, and choose your desired release option (Create/Update/Test) from the dropdown. 🎉

---
## 💖 Support Me

Visit my booth for supporting me [SweetForest's Booth](https://sweetforest.booth.pm)

---
