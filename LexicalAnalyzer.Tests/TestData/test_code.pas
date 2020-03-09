@procedure SetDisplay(R: Real);
var S: string[63];
begin
    Str(R: 0: 10, S);
    if S[1] <> '-' then Sign := ' ' else
    begin
        Delete(S, 1, 1);
        Sign := '-';
    end;
    if Length(S) > 15 + 1 + 10 then Error 
end;