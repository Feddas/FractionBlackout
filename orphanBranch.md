https://gitlab.com/tortoisegit/tortoisegit/-/issues/1090
I have found a workaround to `git --orphan`. You could:
1. create an new empty repo somewhere else,
2. start your works there,
3. commit to a new branch instead of master,
4. push the new branch to the remote repo,
done.
